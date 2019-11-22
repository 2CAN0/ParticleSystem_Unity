using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ParticleSystem))]
public class W_ParticleSystem : Editor
{
    [SerializeField] Texture myTexture;
    GUIStyle iconStyle;
    GUIStyle titleStyle;
    GUIStyle labelStyle;
    ParticleSystem ps;

    bool intialized = false;

    public override void OnInspectorGUI()
    {
        ps = target as ParticleSystem;

        if (!intialized)
        {
            if (ps.GetComponent<BoxCollider>() == null)
                ps.gameObject.AddComponent<BoxCollider>();

            ps.GetComponent<BoxCollider>().hideFlags = HideFlags.HideInInspector;

            iconStyle = new GUIStyle();
            iconStyle.alignment = TextAnchor.MiddleCenter;
            iconStyle.stretchHeight = true;

            titleStyle = new GUIStyle();
            titleStyle.alignment = TextAnchor.LowerCenter;
            titleStyle.fontSize = 45;
            titleStyle.fontStyle = FontStyle.Bold;

            labelStyle = new GUIStyle();
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.fontSize = 12;

            intialized = true;
        }

        ///// Header /////
        GUILayout.BeginHorizontal();

        GUILayout.Label(myTexture, iconStyle);
        GUILayout.Label("Particle System", titleStyle);

        GUILayout.EndHorizontal();
        //////////////////

        GUILayout.Space(30);

        ///// Particle Settings /////
        GUILayout.Label("Particle Settings", labelStyle);

        ps.ParticleMode = (ParticleType)EditorGUILayout.EnumPopup("Particle Type", ps.ParticleMode);

        if (ps.ParticleMode == ParticleType.MeshParticle)
            ps.Particle = (GameObject)EditorGUILayout.ObjectField("Particle", ps.Particle, typeof(GameObject), false);
        else if (ps.ParticleMode == ParticleType.Billboard)
        {
            GUILayout.Space(10);
            ps.ParticleSprite = (Sprite)EditorGUILayout.ObjectField("Particle", ps.ParticleSprite, typeof(Sprite), false);

            // Particle Color Settings
            Color pColor = ps.ParticleColor;
            pColor = EditorGUILayout.ColorField("Particle Tint", pColor);
            // TODO: Density Slider
            ps.ParticleColor = pColor;
        }

        ps.Lifetime = EditorGUILayout.FloatField("Partice lifetime", ps.Lifetime);

        EditorGUILayout.HelpBox("The lifetime of a particle in seconds.", MessageType.None);

        ps.Scale = EditorGUILayout.FloatField("Particale Scale", ps.Scale);
        /////////////////////////////

        GUILayout.Space(20);

        ///// System Settings /////
        GUILayout.Label("Particle System Settings", labelStyle);

        ps.MaxParticle = EditorGUILayout.IntField("Max Particles", ps.MaxParticle);

        ps.GetComponent<BoxCollider>().size = EditorGUILayout.Vector3Field("Spawnbox size", ps.GetComponent<BoxCollider>().size);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("loop"));

        ps.Velocity = EditorGUILayout.Vector3Field("Velocity", ps.Velocity);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotate"));
        EditorGUILayout.HelpBox("Do you want your particles to be rotating?\n(The use of rotation has a big impact on the games performance!)", MessageType.None);

        if (ps.Rotate)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rotationSpeed"));
        }
        //////////////////////////

        serializedObject.ApplyModifiedProperties();
    }
}
