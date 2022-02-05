using System;
using System.Collections.Generic;
namespace Evalua_1
{
    /* 
    Requerimiento 1: Ajustar la salida del printf: Quitar dobles comillas, 
                     ejecutar las setencias de escape(\n, \t) y 
                     asociar la lista de variables a la salidas % 
    Requerimiento 2: Cuando una variable no este declarada levantar excepcion (Tipos.identificador)
    Requerimiento 3: Asignar los valores del scanf a las variables correspondientes
    Requerimiento 4: Agregar el do-While
    */
    public class Lenguaje:Sintaxis
    {
        List<Variable> LV; //declaracion 
        Stack<float> SE; 
        public Lenguaje()
        {
            LV=new List<Variable>(); //instancio el objeto lista
            SE=new Stack<float>(); //instacia el objeto stack
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
        private bool Existe(string nombre)
        {
            foreach (Variable L in LV)
            {
                if(L.getNombre()== nombre)
                {
                    return true;
                }    
            }
            return false; //no existe la variable
        }
        private void Modifica(string nombre, float valor)
        {
            foreach (Variable L in LV)
            {
                if(L.getNombre()== nombre)
                {
                    L.setValor(valor); 
                }    
            }
        }
        private float GetValor(string nombre)
        {
            foreach (Variable L in LV)
            {
                if(L.getNombre()== nombre)
                {
                   return L.getValor(); 
                }    
            }
            return 0;
        }

        //ListaIdentificadores ->  identificador (,ListaIdentificadores)?
        private void ListaIdentificadores(Variable.TDatos tipo)
        {
            if(tipo != Variable.TDatos.sinTipo)
            {
                if(!Existe(getContenido())) 
                {
                    LV.Add(new Variable(getContenido(), tipo));   
                }
                else 
                {
                    throw new Error("ERROR DE SINTAXIS: Variable duplicada: " + getContenido(),linea,log);
                }    
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
            Console.WriteLine(getContenido());
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
            if(!Existe(getContenido())) //buscarlos en Match(Tipos.identificador) y agregar excepcion
            {
                throw new Error("ERROR DE SINTAXIS: Variable no declarada: " + getContenido(),linea,log);
            }
            float valor=float.Parse(Console.ReadLine());//lo asigna 
            Modifica(getContenido(),valor);
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
            bool evalua=Condicion();//si es falsa o verdadero la condicion y se guarda en evalua
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
        private bool Condicion()
        {
            //considerando la negacion 
            if(getContenido()=="!")
            {
                Match("!");
            }
            Expresion();
            string operador = getContenido();
            Match(Tipos.opRelacional);
            Expresion();
            float Resultado1=SE.Pop(); //variable del resultado de operacion
            float Resultado2=SE.Pop(); //variable del resultado de operacion

            //evalauar la condicion que regresa falso o verdadero 
            switch(operador)
            {
                case "==": return Resultado1==Resultado2; 
                case ">=": return Resultado1>=Resultado2; 
                case ">":  return Resultado1>Resultado2; 
                case "<=": return Resultado1<=Resultado2; 
                case "<":  return Resultado1<Resultado2; 
                default:   return Resultado1!=Resultado2; 
            }
        }
        // ***For -> for(identificador=Expresion; Condicion; identificador incTermino) BloqueInstrucciones | Instruccion
        private void For()
        {
            Match("for");
            Match("(");
            if(!Existe(getContenido())) 
            {
                throw new Error("ERROR DE SINTAXIS: Variable no declarada: " + getContenido(),linea,log);
            }
            //float valor=float.Parse(Console.ReadLine());//lo asigna 
            //Modifica(getContenido(),valor);
            //------------------------------------------------------------------
            string variable=getContenido();
            Match(Tipos.identificador);
            Match("=");
            Expresion();
            float Resultado=SE.Pop(); //variable del resultado de operacion
            Modifica(variable,Resultado); 
            Match(Tipos.finSentencia);
            bool evalua=Condicion();
            Match(Tipos.finSentencia);
            string variable2=getContenido(); 
            Match(Tipos.identificador); 
            string operador=getContenido();
            Match(Tipos.incTermino);
            if(operador=="++")
            {
                Modifica(variable2,GetValor(variable2)+1);
            }
            else if(operador=="--")
            {
                Modifica(variable2,GetValor(variable2)-1);
            }
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
            bool evalua=Condicion();
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
            float Resultado=SE.Pop(); //variable del resultado de operacion no se modifica variable
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
            string variable=getContenido();
            Match(Tipos.identificador);
            Match(Tipos.asignacion);
            Expresion();
            float Resultado=SE.Pop(); //variable del resultado de operacion 
            Modifica(variable,Resultado);
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
                string operador=getContenido();
                Match(Tipos.opTermino);
                Termino();// segundo termino 
                //Console.Write(operador + " ");
                float N1=SE.Pop(); //sacamos elementos del stack para seguir con operaciones
                float N2=SE.Pop();
                switch (operador)
                {
                    case "+": SE.Push(N2+N1); break;
                    case "-": SE.Push(N2-N1); break;
                }
            }
        }
        // Termino	  -> Factor PorFactor 
        private void Termino()
        {
            Factor(); //primer factor
            PorFactor();
        }
        // PorFactor  -> (opFactor Factor)?
        private void PorFactor()
        {
            if(getClasificacion()==Tipos.opFactor)
            {
                string operador=getContenido();
                Match(Tipos.opFactor);
                Factor(); //segundo factor
                //Console.Write(operador + " ");
                float N1=SE.Pop(); //sacamos elementos del stack para seguir con operaciones
                float N2=SE.Pop();
                switch (operador)
                {
                    case "*": SE.Push(N2*N1); break;
                    case "/": SE.Push(N2/N1); break;
                }
            }
        }
        // Factor	  -> numero | identificador | (Expresion) 
        private void Factor()
        {
            if(getClasificacion()==Tipos.numero)
           {
               //Console.Write(getContenido()+ " "); 
               SE.Push(float.Parse(getContenido())); //metemos al stack el numero
               Match(Tipos.numero);
           }
           else if(getClasificacion()==Tipos.identificador)
           {
               //Console.Write(getContenido()+ " ");
               SE.Push(GetValor(getContenido())); //metemos al stack la variable
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