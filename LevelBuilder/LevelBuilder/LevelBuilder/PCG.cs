using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelBuilder
{
    internal class PCG
    {

        public List<MarkovColumn>? GenerateChains(string path, string name, int N)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            List<MarkovColumn> result = new List<MarkovColumn>();
            List<string> textColumns = new List<string>();

            if (dir.Exists)
            {
                FileInfo[] fi = dir.GetFiles();

                for (int i = 0; i < fi.Length; i++)
                {
                    textColumns.AddRange(PrepareTrainingData(fi[i].OpenText()));
                }

                result = TrainData(textColumns, N);
            }
            else
            {
                Console.WriteLine("ERROR: Directorio no encontrado");
                return null;
            }

            Console.WriteLine("Cadenas generadas");

            SaveMkChains(result, name);

            return result;
        }

        List<string> PrepareTrainingData(StreamReader sr)
        {
            //StreamReader sr = new StreamReader(path);

            List<string> trainingLines = new List<string>();
            List<string> trainingColumns = new List<string>();

            //Leer todas las lineas validas
            string? readLine;
            while ((readLine = sr.ReadLine()) != null)
            {
                trainingLines.Add(readLine);
            }

            //Extraer las columnas de las lineas
            string[] trainingColumnsArr = new string[trainingLines[0].Length];
            for (int i = 0; i < trainingColumnsArr.Length; i++)
            {
                for (int j = 0; j < trainingLines.Count; j++)
                {
                    trainingColumnsArr[i] += trainingLines[j][i];
                }
                trainingColumns.Add(trainingColumnsArr[i]);
            }

            return trainingColumns;
        }

        List<MarkovColumn> TrainData(List<string> trainingInfo, int N)
        {
            //Valor de retorno
            List<MarkovColumn> trainedData = new List<MarkovColumn>();

            //La funcion revisara cada columna de trainingInfo (excepto la ultima). La comparara con las columnas actualmente en trainedData.
            //Su no hay una columna igual en trainedData, la agregará con su sucesor en la lista de transiciones.
            //Si hay una columna igual, agregará a su sucesor como transición (esto para mantener la cuenta de las pobabilidades).
            for (int i = 0; i < trainingInfo.Count - N; i += N)
            {
                bool simileFound = false;

                //Revisar columnas ya ingresadas
                foreach (MarkovColumn column in trainedData)
                {
                    //Comparar columnas
                    bool same = true;
                    for(int a = 0; a < N; a++)
                    {
                        if (column.columnStr[a] != trainingInfo[i + a])
                        {
                            same = false;
                            break;
                        }
                    }

                    //Anadir otro sucesor
                    if (same)
                    {
                        string[] newColstr = new string[N];
                        for(int a = 0; a < N; a++)
                        {
                            //Asegurarse que no se pase del rango
                            if (i + a + N >= trainingInfo.Count)
                            {
                                newColstr[a] = "-------------X";
                                continue;
                            }

                            //Añadir string a la columna
                            newColstr[a] = trainingInfo[i + a + N];
                        }
                        column.AddSuccesor(new MarkovColumn(newColstr));

                        simileFound = true;
                        break;
                    }
                }

                //Si no hay columnas iguales, crea una nueva
                if (!simileFound)
                {
                    //Crear dos Markov Columns, una para la nueva adicion y una para su transicion.
                    string[] newColstr = new string[N];
                    for (int a = 0; a < N; a++)
                    {
                        //Asegurarse que no se pase del rango
                        if (i + a >= trainingInfo.Count)
                        {
                            newColstr[a] = "-------------X";
                            continue;
                        }

                        //Añadir string a la columna
                        newColstr[a] = trainingInfo[i + a];
                    }
                    MarkovColumn newColumn = new(newColstr);

                    string[] newSuccesorstr = new string[N];
                    for (int a = 0; a < N; a++)
                    {
                        //Asegurarse que no se pase del rango
                        if (i + a + N >= trainingInfo.Count)
                        {
                            newColstr[a] = "-------------X";
                            continue;
                        }

                        //Añadir string a la columna
                        newSuccesorstr[a] = trainingInfo[i + a + N];
                    }

                    newColumn.AddSuccesor(new MarkovColumn(newSuccesorstr));
                    trainedData.Add(newColumn);
                }
            }

            return trainedData;
        }

        public string[]? CreateLevel(int lenght, List<MarkovColumn> mkColumns)
        {
            string[] strLevel = new string[lenght];
            Random rand = new Random();
            int N = mkColumns[0].columnStr.Length;

            //Primera columna
            //El texto sugiere hacer una eleccion aleatoria con probabilidades entre todas las columnas, pero creo que copiar la primera del nivel de muestra es mas apropiada para niveles de mario.
            for(int i = 0; i < N; i++)
            {
                strLevel[i] = mkColumns[0].columnStr[i];
            }

            //Generar el resto de columnas en base a la probabilidad de transiciones
            //Como transitionList tiene duplicados para las transiciones, es mas probable que se generen aquellas que aparecian mas en el nivel de ejemplo, cumpliendo asi con la premisa.
            for (int i = N; i < lenght; i += N)
            {
                //Encontrar la columna a la que pertenece el string
                MarkovColumn? col = null;
                foreach (MarkovColumn column in mkColumns)
                {
                    //Coomparar
                    bool same = true;
                    for(int a = 0; a < N; a++)
                    {
                        if (strLevel[i - N + a] != column.columnStr[a])
                        {
                            same = false;
                            break;
                        }
                    }

                    if (same)
                    {
                        col = column;
                        break;
                    }
                }

                //En caso de error
                string[] err = { "ERROR: Columna sin registrar generada." };
                if (col == null) { Console.WriteLine(err); return null; }

                int transitionCount = col.transitionList.Count;
                int choice = rand.Next(0, transitionCount);

                for (int a = 0; a < N; a++)
                {
                    if(i + a > lenght) { break; }

                    strLevel[i + a] = col.transitionList[choice].columnStr[a];
                }
            }

            //Agregar parte final
            string[]? finalLevel = AppendFinisher(strLevel);
            if(finalLevel == null)
            {
                return strLevel;
            }
            return finalLevel;

        }

        public void WriteLevel(string path, string[] columns)
        {
            string s = "";

            //Altura
            for (int h = 0; h < columns[0].Length; h++)
            {
                //Largo
                for (int w = 0; w < columns.Length; w++)
                {
                    s += columns[w][h];
                }
                s += '\n';
            }

            File.Delete(path);
            File.WriteAllText(path, s);
        }

        bool SaveMkChains(List<MarkovColumn> mkChains, string name)
        {
            //Comprobar si la carpeta para guardar cadenas existe
            DirectoryInfo dir = new DirectoryInfo("GeneratedChains");
            if (dir.Exists)
            {
                List<string> lines = new List<string>();
                int N = mkChains[0].columnStr.Length;

                foreach(MarkovColumn chain in mkChains)
                {
                    //Escribir columna
                    for(int i = 0; i < N; i++)
                    {
                        lines.Add(chain.columnStr[i]);
                    }

                    //Escribir transiciones
                    foreach(MarkovColumn transition in chain.transitionList)
                    {
                        for (int i = 0; i < N; i++)
                        {
                            lines.Add("TRANSITION:" + transition.columnStr[i]);
                        }
                    }
                }

                //Crear archivo
                File.WriteAllLines("GeneratedChains/" + name + ".txt", lines);
            }
            else
            {
                Console.WriteLine("ERROR: Carpeta \"GeneratedChains\" no encontrada, asegurese de que esta exista.");
                return false;
            }

            Console.WriteLine("Cadenas guardadas satisfactoriamente.");
            return true;
        }

        public List<MarkovColumn>? ReadMkChains(string name)
        {
            DirectoryInfo dir = new DirectoryInfo("GeneratedChains");
            List<MarkovColumn> markovColumns = new List<MarkovColumn>();

            //Comprobar si la carpeta existe
            if (dir.Exists)
            {
                //Comprobar si el archivo existe
                StreamReader sr;
                try
                {
                    sr = new StreamReader("GeneratedChains/" + name + ".txt");
                }
                catch
                {
                    Console.WriteLine("ERROR: Archivo no encontrado.");
                    return null;
                }

                string? line = sr.ReadLine();
                if(line == null) { Console.WriteLine("ERROR: Archivo nulo."); return null; }

                //Conseguir el grosor de las cadenas y leer primera columna
                List<string> aux = new List<string>();
                int N = 0;
                while (!line.StartsWith("TRANSITION:"))
                {
                    aux.Add(line);
                    N++;

                    line = sr.ReadLine();
                    if(line == null) { Console.WriteLine("ALERTA: Linea nula. 0"); break; }
                }
                Console.WriteLine("Leyendo coleccion de cadenas de grosor " + N);

                //Crear primera columna
                string[] newCol = new string[N];
                for (int i = 0; i < N; i++)
                {
                    newCol[i] = aux[i];
                }
                MarkovColumn mkColumn = new MarkovColumn(newCol);


                //Leer el resto de las cadenas
                while(line == null)
                {
                    line = sr.ReadLine();
                    if (line == null) { Console.WriteLine("ALERTA: Linea nula. 1"); }
                }

                while (!sr.EndOfStream)
                {
                    if(line == null) { Console.WriteLine("ALERTA: Linea nula. 4"); continue; }

                    //Columna nueva
                    if (!line.StartsWith("TRANSITION:"))
                    {
                        //Añadir anterior
                        markovColumns.Add(mkColumn);

                        //Registrar linea actual
                        line = line.Trim();

                        newCol = new string[N];
                        newCol[0] = line;

                        //Registrar lineas siguientes
                        for (int i = 1; i < N; i++)
                        {
                            line = sr.ReadLine();
                            if (line == null) { Console.WriteLine("ALERTA: Linea nula. 2"); continue; }

                            line = line.Trim();
                            newCol[i] = line;
                        }
                        mkColumn = new MarkovColumn(newCol);
                    }
                    //Transicion
                    else
                    {
                        //Registrar linea actual
                        line = line.Replace("TRANSITION:", "");
                        line = line.Trim();

                        string[] newTrans = new string[N];
                        newTrans[0] = line;

                        //Registrar lineas siguientes
                        for (int i = 1; i < N; i++)
                        {
                            line = sr.ReadLine();
                            if (line == null) { Console.WriteLine("ALERTA: Linea nula. 3"); continue; }

                            line = line.Replace("TRANSITION:", "");
                            line = line.Trim();
                            newTrans[i] = line;
                        }
                        mkColumn.transitionList.Add(new MarkovColumn(newTrans));
                    }

                    line = sr.ReadLine();
                }
                //Añadir ultima
                markovColumns.Add(mkColumn);
            }
            else
            {
                Console.WriteLine("ERROR: Carpeta \"GeneratedChains\" no encontrada, asegurese de que esta exista.");
                return null;
            }
            return markovColumns;
        }

        public void PrintMarkovChains(List<MarkovColumn> markovColumns)
        {
            int N = markovColumns[0].columnStr.Length;
            Console.WriteLine("Imprimiendo columnas de grosor " + N + "...");

            foreach(MarkovColumn col in markovColumns)
            {
                for(int i = 0; i < N; i++)
                {
                    Console.WriteLine(col.columnStr[i]);
                }

                foreach(MarkovColumn trans in col.transitionList)
                {
                    for (int i = 0; i < N; i++)
                    {
                        Console.WriteLine("Transicion:" + trans.columnStr[i]);
                    }
                }

            }
        }

        string[]? AppendFinisher(string[] baseLevel)
        {
            //Ingresar direccion
            Console.WriteLine("Ingrese direccion del final que quiera para su nivel");
            string? path;
            do
            {
                path = Console.ReadLine();
                if (path == null) { Console.WriteLine("ERROR: Ingrese un valor no nulo"); }
            } while (path == null);

            //Encontrar el archivo
            StreamReader sr;
            try
            {
                sr = new StreamReader(path + ".txt");
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR: " + e);
                return null;
            }

            //Convertir final en columnas
            List<string> finisher = PrepareTrainingData(sr);

            //Agregar al final del nivel
            string[] newLevel = new string[baseLevel.Length + finisher.Count];
            for (int i = 0; i < baseLevel.Length; i++)
            {
                newLevel[i] = baseLevel[i];
            }

            for (int i = 0; i < finisher.Count; i++)
            {
                newLevel[baseLevel.Length + i] = finisher[i];
            }

            return newLevel;

        }
    }
}
