using AlgeTool;
namespace AlgeTool;
public class Vector
{

	private float[] vector;		
	
	public float this[int index]{
	get{ 
		if(index< this.Size && index >=0){
			return vector[index];
		}else{
		throw new System.Exception("Index out of Range");
		}}

	set{ 
		if(index<this.Size && index>=0){
			 vector[index]=value;
		}else{
		throw new System.Exception("Index out of Range");

		}}
	}
	public int Size{ get{return this.vector.Length; }}
	public float Norm{ get{return(float) Math.Sqrt(DotProduct(this,this));}}

	public Vector(float[] vector){
		this.vector = vector;
	}

	public static float CosDistance(Vector vec1, Vector vec2)
	{
		CheckNullVector(vec1);
		CheckNullVector(vec2);

		return DotProduct(vec1,vec2)/vec1.Norm*vec2.Norm;

	}

	public Vector(int dimention){
	this.vector= new float[dimention];
	}


	public static Vector operator +(Vector v1, Vector v2){
		return Sum(v1,v2);
	}
	public static float operator *(Vector v1,Vector v2){
		return DotProduct(v1,v2);
	}
	public static Vector operator *(Vector v1,float scalar){
		return ScalarProduct(v1,scalar); 
	}
	public static Vector operator *(float scalar,Vector v1){
		return ScalarProduct(v1,scalar); 
	}
	public static Vector operator -(Vector v1,Vector v2){
		return v1+(v2*-1);
	}

private static void CheckNullVector(Vector v1){ if( v1==null) throw new System.Exception("Vector no puede ser null");}
private static bool Check_Dimentions(Vector v1,Vector v2){ return v1.Size==v2.Size;}
	//metodos auxiliares
private static Vector Sum(Vector vec1,Vector vect2){
	CheckNullVector(vec1);
	CheckNullVector(vect2);
	Check_Dimentions(vec1,vect2);
	Vector	restult = new Vector(vec1.Size);

	for(int i=0;i< restult.Size;i++){
		restult[i]= vec1[i]+vect2[i];
	}
	return restult;
}

private static Vector ScalarProduct(Vector v1, float scalar){
	var result = new Vector(v1.Size);

	for(int i=0;i<result.Size;i++){
		result[i]=v1[i]*scalar;
	}
	return result;
}

private static float DotProduct(Vector vec1,Vector vect2){
	CheckNullVector(vec1);
	CheckNullVector(vect2);
	Check_Dimentions(vec1,vect2);
		float restult=0;
	for(int i=0;i<vec1.Size;i++){
		restult+= vec1[i]*vect2[i];
	}
		return restult;	
}

}
