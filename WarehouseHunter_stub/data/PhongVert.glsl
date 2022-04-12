#define PROCESSING_TEXLIGHT_SHADER
uniform int lightCount;
uniform vec3 lightDiffuse[8];
uniform vec4 lightPosition[8];


attribute vec4 vertex;
attribute vec4 color;
attribute vec3 normal;
attribute vec2 texCoord;

uniform mat4 modelview;
uniform mat4 transform;
uniform mat3 normalMatrix;
uniform mat4 texMatrix;

varying vec3 transformedNormal;
varying vec3 vertexCamera;
varying vec4 vertColor;
varying vec3 lightDir[8];
varying vec4 outputTexCoord;

void main(void)  
{     
vertColor = color;
   gl_Position = transform * vertex;
   
    outputTexCoord.xy = texCoord;
	outputTexCoord.zw = vec2(1.0, 1.0);
	outputTexCoord = texMatrix*outputTexCoord;
   
   vertexCamera = vec3(modelview * vertex);       
   transformedNormal = normalize(normalMatrix * normal);
   for (int i=0;i<lightCount;i++)
   lightDir[i] = normalize(lightPosition[i].xyz - vertexCamera);
     
}