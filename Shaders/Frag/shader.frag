#version 330 core

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Material material;
uniform Light lights[10];
uniform vec3 viewPos;
uniform sampler2D texture0;
uniform sampler2D shadowMap;
uniform sampler2D normalMap;
uniform sampler2D metallicMap;
uniform int lightsCount;
uniform int hasNormalMap;

in vec2 texCoord;
in vec3 Tangent;
in vec3 Bitangent;
in vec3 Normal;
in vec3 FragPos;
in vec4 FragPosLightSpace;

out vec4 outputColor;

float ShadowCalculation(vec4 fragPosLightSpace, vec3 normal, vec3 lightDir)
{
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    projCoords = projCoords * 0.5 + 0.5;
    
    if(projCoords.z > 1.0)
        return 0.0;
    
    float closestDepth = texture(shadowMap, projCoords.xy).r;
    float currentDepth = projCoords.z;
    float bias = max(0.05 * (1.0 - dot(normal, lightDir)), 0.005);
    
    float shadow = 0.0;
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
    
    for(int x = -1; x <= 1; ++x)
    {
        for(int y = -1; y <= 1; ++y)
        {
            float pcfDepth = texture(shadowMap, projCoords.xy + vec2(x, y) * texelSize).r;
            shadow += currentDepth - bias > pcfDepth ? 1.0 : 0.0;        
        }
    }
    shadow /= 9.0;
    
    return shadow;
}

void main()
{
    vec4 textureColor = texture(texture0, texCoord);

    vec3 norm;
    if (hasNormalMap == 1) {
        vec3 T = normalize(Tangent);
        vec3 N = normalize(Normal);
        vec3 B = cross(N, T);
    
        vec3 normalFromMap = texture(normalMap, texCoord).rgb;
        normalFromMap = normalize(normalFromMap * 2.0 - 1.0);
    
        mat3 TBN = mat3(T, B, N);
        norm = normalize(TBN * normalFromMap);
    }
    else {
        norm = normalize(Normal);
    }

    vec3 viewDir = normalize(viewPos - FragPos);

    vec3 ambient = lights[0].ambient * material.ambient;

    float metallic = texture(metallicMap, texCoord).r;

    vec3 mainResult = ambient * textureColor.rgb;

    for(int i = 0; i < lightsCount; i++) {
        vec3 lightDir = normalize(lights[i].position - FragPos);

        float shadow = ShadowCalculation(FragPosLightSpace, norm, lightDir);
        float diff = max(dot(norm, lightDir), 0.0);

        vec3 diffuse = lights[i].diffuse * diff * material.diffuse;
        vec3 reflectDir = -reflect(lightDir, norm);

        float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

        vec3 specular = lights[i].specular * spec * material.specular;

        vec3 albedo = textureColor.rgb;
        vec3 F0 = mix(vec3(0.04), albedo, metallic);
        vec3 F = F0 + (1.0 - F0) * pow(1.0 - max(dot(viewDir, reflectDir), 0.0), 5.0);
        vec3 specularFinal = specular * F;
        vec3 result = (1.0 - shadow) * (diffuse + specularFinal) * albedo;

        mainResult += result;
    }

    outputColor = vec4(mainResult, textureColor.a);
}