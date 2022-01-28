using System;
using System.Collections.Generic;
namespace Evalua
{
    /* 
    Requerimiento 1: Ajustar la salida del printf: Quitar dobles comillas 
                     y asociar la lista de variables a la salidas % 
    Requerimiento 2: Asignar los valores del scanf a las variables correspondientes
    */
    public class Lenguaje:Sintaxis
    {
        List<Variable> LV; //declaracion 
        public Lenguaje()
        {
            LV=new List<Variable>(); //instancio el objeto
        }
        // Programa	-> 	Librerias Variables Main
        public void Programa()
        {
            Librerias();
            Variables();
            Main();
            ImprimeLista();
        }
        // Librerias->	#include<identificador(.h)?> Librerias?
        private void Librerias()
        {
            Match("#");
            Match("include");
            Match("<");
            Match(Tipos.identificador);
            if(getContenido()==".")
            {
                Match(".");
                Match("h");
            }
            Match(">");
            if(getContenido()=="#")
            {
                Librerias();
            }
        }
        private Variable.TDatos StrignToEnum(string tipo)
        {
            switch(tipo)
            {
                case "char": return Variable.TDatos.CHAR;
                case "int": return Variable.TDatos.INT;
                case "float": return Variable.TDatos.FLOAT;
                default: return Variable.TDatos.sinTipo;
            }
        }
        //Variables ->  tipoDato ListaIdentificadores; Variables?
        private void Variables()
        {
            Variable.TDatos tipo=Variable.TDatos.CHAR; //inicializamos=que cero
            tipo=StrignToEnum(getContenido());
            Match(Tipos.tipoDato);
            ListaIdentificadores(tipo);
            Match(Tipos.finSentencia);
            if(getClasificacion()==Tipos.tipoDato) //recursividad de Variables
            {
                Variables();
            }
        }
        private void ImprimeLista()
        {
            log.WriteLine("Lista de Variables");
            foreach (Variable L in LV)
            {
                log.WriteLine(L.getNombre() + " "+ L.getTipoDato() + " "+ L.getValor());
            }
        }
        //ListaIdentificadores ->  identificador (,ListaIdentificadores)?
        private void ListaIdentificadores(Variable.TDatos tipo)
        {
            if(tipo != Variable.TDatos.sinTipo)
            {
                LV.Add(new Variable(getContenido(), tipo));   
            }
            Match(Tipos.identificador);
            if(getContenido()==",")//recursividad de lista
            {
                Match(",");
                ListaIdentificadores(tipo);
            }
        }
        // Main  ->	void main() BloqueInstrucciones
        private void Main()
        {
            Match("void");
            Match("main");
            Match("(");
            Match(")");
            BloqueInstrucciones();
        }
        // BloqueInstrucciones ->  {ListaInstrucciones}
        private void BloqueInstrucciones()
        {
            Match("{");
            ListaInstrucciones();
            Match("}");
        }
        // ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones()
        {
            Instruccion();
            if(getContenido() != "}") 
            {
                ListaInstrucciones();
            }
    
        }
        // Instrccion  -> Printf  | Scanf | If | For | While | Switch | Asignacion 
        private void Instruccion()
        {
            if(getContenido() == "printf")
            {
                Printf();
            }
            else if(getContenido() == "scanf")
            {
                Scanf();
            }
            else if(getContenido() == "if")
            {
                If();
            }
            else if(getContenido() == "for")
            {
                For();
            }
            else if(getContenido() == "while")
            {
                While();
            }
            else if(getContenido() == "switch")
            {
                Switch();
            }
            else
            {
                Asignacion();
            }
        }
        // Printf -> printf(cadena (,ListaIdentificadores)?);
        private void Printf()
        {
            Match("printf");
            Match("(");
            Console.Write(getContenido());
            Match(Tipos.Cadena);
            if(getContenido() == ",")
            {
                Match(",");
                ListaIdentificadores(Variable.TDatos.sinTipo);
            }
            Match(")");
            Match(Tipos.finSentencia);
        } 
        // Scanf -> scanf(cadena,ListadeAmpersas);
        private void Scanf()
        {
            Match("scanf");
            Match("(");
            Match(Tipos.Cadena);
            Match(",");
            ListadeAmpersas();
            Match(")");
            Match(Tipos.finSentencia);
        } 
        // ListadeAmpersas -> & identificador(,ListadeAmpersas)?
        private void ListadeAmpersas()
        {
            Match("&");
            Console.ReadLine();
            Match(Tipos.identificador);
            if(getContenido()==",")
            {
                Match(",");
                ListadeAmpersas();
            }
        }
        // If -> if(Condicion) BloqueInstrucciones | Intruccion (else BloqueInstrcciones | Instruccion)?
        private void If()
        {
            Match("if");
            Match("(");
            Condicion();
            Match(")");
            if(getContenido()!="{")
            {
                Instruccion();
            }
            else
            {
                BloqueInstrucciones();
            }
            //? 
            if(getContenido()=="else")
            {
                Match("else");
                if(getContenido()!="{")
                {
                    Instruccion();
                }
                else
                {
                    BloqueInstrucciones();
                }
            }
        }
        // ***Condicion -> Expresion oprRelacional Expresion  
        private void Condicion()
        {
            //considerando la negacion 
            if(getContenido()=="!")
            {
                Match("!");
            }
            Expresion();
            Match(Tipos.opRelacional);
            Expresion();
        }
        // ***For -> for(identificador=Expresion; Condicion; identificador incTermino) BloqueInstrucciones | Instruccion
        private void For()
        {
            Match("for");
            Match("(");
            Match(Tipos.identificador);
            Match("=");
            Expresion();
            Match(Tipos.finSentencia);
            Condicion();
            Match(Tipos.finSentencia);
            Match(Tipos.identificador);
            Match(Tipos.incTermino);
            Match(")");
            if(getContenido()!="{")
            {
                Instruccion();
            }
            else
            {
                BloqueInstrucciones();
            }
        }
        // While -> while(Condicion) BloqueInstrucciones | Instruccion
        private void While()
        {
            Match("while");
            Match("(");
            Condicion();
            Match(")");
            if(getContenido()!="{") // bloque de instrucciones y instrucciones
            {
                Instruccion();
            }
            else
            {
                BloqueInstrucciones();
            }
        }
        // Switch -> switch(Expresion)  {VariosCase Default}
        private void Switch()
        {
            Match("switch");
            Match("(");
            Expresion();
            Match(")");
            Match("{");
            VariosCase();
            Default();
            Match("}");
        }
        // VariosCase -> case numero: VariosCase | CaseInstruccion
        private void VariosCase()
        {
            Match("case");  
            Match(Tipos.numero);
            Match(":");
            if(getContenido() != "case")
            {
                CaseInstruccion();
            }
            else
            {
                VariosCase();
            } 
        }
        // CaseInstruccion  -> Instruccion| BloqueInstrucciones Break 
        private void CaseInstruccion()
        {
            if(getContenido()!="{")
            {
                Instruccion();
            }
            else
            {
                BloqueInstrucciones();
            }
            Break();
        }
        //Break -> break;? VariosCase
        private void Break()
        {
            if(getContenido() == "break")
            {
                Match("break");
                Match(Tipos.finSentencia);
                if(getContenido() == "case")
                {
                    VariosCase();
                }
            }
        } 
        // Default-> default:? Intruccion | BloqueInstrcciones 
        private void Default()
        {
            if(getContenido()=="default")
            {
                Match("default");
                Match(":");
                if(getContenido()!="{")
                {
                    Instruccion();
                }
                else
                {
                    BloqueInstrucciones();
                }
            }   
        }
        // Asignacion  -> identificador = Expresion;
        private void Asignacion()
        {
            Match(Tipos.identificador);
            Match(Tipos.asignacion);
            Expresion();
            Match(Tipos.finSentencia);
        }
        // Expresion  -> Termino MasTermino 
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        // MasTermino -> (opTermino Termino)? 
        private void MasTermino()
        {
            if(getClasificacion()==Tipos.opTermino)
            {
                Match(Tipos.opTermino);
                Termino();
            }
        }
        // Termino	  -> Factor PorFactor 
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        // PorFactor  -> (opFactor Factor)?
        private void PorFactor()
        {
            if(getClasificacion()==Tipos.opFactor)
            {
                Match(Tipos.opFactor);
                Factor();
            }
        }
        // Factor	  -> numero | identificador | (Expresion) 
        private void Factor()
        {
            if(getClasificacion()==Tipos.numero)
           {
               Match(Tipos.numero);
           }
           else if(getClasificacion()==Tipos.identificador)
           {
               Match(Tipos.identificador);
           }
           else
           {
               Match("(");
               Expresion();
               Match(")");
           }
        }
    }
}