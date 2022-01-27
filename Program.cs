using System;

namespace Evalua
{
    class Program
    {
        static void Main(string[] args)
        {
            Lenguaje L = new Lenguaje();
           
            try
            {
                L.Programa();
                /*while(!L.FinAchivo())
                {
                    L.NextToken();
                }*/
            }
            catch (Exception)
            {
               Console.WriteLine("Fin de Compilacion. Verifique el codigo");
            }
        }
           

    }
}
