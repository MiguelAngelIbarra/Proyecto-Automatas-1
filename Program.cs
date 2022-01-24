using System;

namespace Sintaxis_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Lenguaje L = new Lenguaje();
           
            try
            {
                L.Programa();
            }
            catch (Exception)
            {
               Console.WriteLine("Fin de Compilacion. Verifique el codigo");
            }
            
           /* while(!L.FinAchivo())
            {
                L.NextToken();
                
            }*/

        }
    }
}
