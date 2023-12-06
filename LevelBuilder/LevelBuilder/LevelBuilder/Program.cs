using LevelBuilder;
using System.Xml.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        PCG pcg = new PCG();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Crear nueva coleccion de MarkovChains");
            Console.WriteLine("2. Generar nuevo nivel");
            Console.WriteLine("3. Resumen de coleccion de MarkovChains");
            Console.WriteLine("4. Imprimir coleccion de MarkovChains");

            string? optionStr;
            do
            {
                optionStr = Console.ReadLine();
                if(optionStr == null) { Console.WriteLine("ERROR: Ingrese un valor no nulo"); }
            } while (optionStr == null);

            int option = int.Parse(optionStr);

            switch (option)
            {
                case 1:
                    try
                    {
                        Console.Clear();
                        Console.WriteLine("/////// CREAR NUEVA COLECCION DE MARKOVCHAINS ///////");

                        Console.WriteLine("DIRECCION DE LA CARPETA CON NIVELES: ");
                        string? dir = Console.ReadLine();
                        if (dir == null) { Console.WriteLine("ERROR: Ingrese un valor no nulo"); break; }

                        Console.WriteLine("INGRESE NOMBRE PARA EXPORTAR LA COLECCION: ");
                        string? name = Console.ReadLine();
                        if (name == null) { Console.WriteLine("ERROR: Ingrese un valor no nulo"); break; }

                        Console.WriteLine("INGRESE GROSOR DE LAS CADENAS: ");
                        string? NStr = Console.ReadLine();
                        if (NStr == null) { Console.WriteLine("ERROR: Ingrese un valor no nulo"); break; }
                        int N = int.Parse(NStr);



                        pcg.GenerateChains(dir, name, N);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("ERROR: " + ex.Message);
                    }

                    break;
                case 2:
                    try
                    {
                        Console.Clear();
                        Console.WriteLine("/////// CREAR NUEVO NIVEL ///////");

                        Console.WriteLine("NOMBRE DE LA COLECCION DE MARKOVCHAINS PARA USAR: ");
                        string? mkname = Console.ReadLine();
                        if (mkname == null) { Console.WriteLine("ERROR: Ingrese un valor no nulo"); break; }

                        Console.WriteLine("INGRESE NOMBRE PARA EXPORTAR EL NIVEL: ");
                        string? lvname = Console.ReadLine();
                        if (lvname == null) { Console.WriteLine("ERROR: Ingrese un valor no nulo"); break; }

                        Console.WriteLine("INGRESE LONGITUD HORIZONTAL DEL NIVEL: ");
                        string? lenghtStr = Console.ReadLine();
                        if (lenghtStr == null) { Console.WriteLine("ERROR: Ingrese un valor no nulo"); break; }
                        int lenght = int.Parse(lenghtStr);



                        Console.WriteLine("Leyendo cadenas...");
                        List<MarkovColumn>? mkCollection = pcg.ReadMkChains(mkname);
                        if (mkCollection == null) { break; }

                        Console.WriteLine("Generando nivel...");
                        string[]? output = pcg.CreateLevel(lenght, mkCollection);
                        if(output == null) { break; }

                        Console.WriteLine("Escribiendo archivo de texto...");
                        pcg.WriteLevel("Levels/Output/" + lvname + ".txt", output);

                        Console.WriteLine("Nivel guardado exitosamente.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR: " + ex.Message);
                    }

                    break;
                case 3:
                    try
                    {
                        Console.WriteLine("/////// RESUMEN DE COLECCION DE MARKOV CHAINS ///////");
                        Console.WriteLine("NOMBRE DE LA COLECCION DE MARKOVCHAINS: ");
                        string? colname = Console.ReadLine();
                        if(colname == null) { Console.WriteLine("ERROR: Ingrese un valor no nulo"); break; }

                        List<MarkovColumn>? mkChains = pcg.ReadMkChains(colname);
                        if (mkChains == null) { break; }

                        Console.WriteLine("Cantindad de columnas: " + mkChains.Count);
                        for(int i = 0; i < mkChains.Count; i++)
                        {
                            Console.WriteLine("Columna: " + i + " - Cantidad de transiciones: " + mkChains[i].transitionList.Count);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR: " + ex.Message);
                    }//*/
                    break;
                case 4:
                    try
                    {
                        Console.WriteLine("/////// IMPRIMIR COLECCION DE MARKOV CHAINS ///////");
                        Console.WriteLine("NOMBRE DE LA COLECCION DE MARKOVCHAINS: ");
                        string? colname = Console.ReadLine();
                        if (colname == null) { Console.WriteLine("ERROR: Ingrese un valor no nulo"); break; }

                        List<MarkovColumn>? mkChains = pcg.ReadMkChains(colname);
                        if (mkChains == null) { break; }

                        pcg.PrintMarkovChains(mkChains);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR: " + ex.Message);
                    }//*/
                    break;
            }

            Console.WriteLine("Pulse enter para continuar...");
            Console.ReadLine();
        }
    }
}