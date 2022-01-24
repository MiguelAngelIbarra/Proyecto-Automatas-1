namespace Sintaxis_1
{
    public class Token
    {
        
        public enum Tipos{
            identificador, numero, caracter, asignacion, finSentencia, 
            opLogico, opRelacional, opTermino, opFactor, 
            incTermino, incFactor, Cadena, inicializacion, 
            tipoDato, zona, condicion, ciclo, ternario, 
            opFlujoEntrada, opFlujoSalida,

    
        }
        string Contenido;
        Tipos Clasificacion;

        public void setContenido(string Contenido)
        {
            this.Contenido=Contenido;
        }
        public void setClasificacion(Tipos Clasificacion)
        {
            this.Clasificacion=Clasificacion;
        }  

        public string getContenido()
        {
            return Contenido;
        }     
        public Tipos getClasificacion()
        {
            return Clasificacion;
        }

    }
}