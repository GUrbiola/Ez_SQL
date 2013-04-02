public class @ClassName
{
	#region Propiedades de la Clase (variables... datos...)
@ClassVars
	#endregion
	
	#region Constructores, basico y completo
	//Constructor Basico
	public @ClassName()
	{
	}
	
	//Contructor con todos los campos
	public @ClassName(@CSharpFields)
	{
@DataAssigment
	}
	#endregion
}
