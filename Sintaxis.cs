using System;

namespace Sintaxis_1
{
    public class Sintaxis:Lexico
    {
        public Sintaxis()
        {
            NextToken();
        }

        public void Match(string Espera) //compara el contenido contra la gramatica  
        {
            if(getContenido()==Espera)
            {
                NextToken();
            }
            else
            {
                throw new Error("ERROR DE SINTAXIS: Se espera un " + Espera,linea,log);
            }
        }

        public void Match(Tipos Espera) //compara la Clasificacion contra la gramatica 
        {
            if(getClasificacion()==Espera)
            {
                NextToken();
            }
            else
            {
                throw new Error("ERROR DE SINTAXIS: Se espera un " + Espera, linea, log);
            }
        }

    }
}