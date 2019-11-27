using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ParticleSystem))]
public class W_ParticleSystem : Editor
{
    [SerializeField] Texture myTexture;
    GUIStyle iconStyle;
    GUIStyle titleStyle;
    GUIStyle labelStyle;
    GUIStyle label2Style;
    ParticleSystem ps;
    Window selectedSetting;

    bool intialized = false;
    enum Window
    {
        particleSettings,
        systemSettings,
        physicsSettings
    }

    public override void OnInspectorGUI()
    {
        ps = target as ParticleSystem;

        if (!intialized)
        {
            selectedSetting = Window.particleSettings;

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
            labelStyle.fontSize = 14;

            label2Style = new GUIStyle();
            label2Style.fontStyle = FontStyle.Bold;
            label2Style.fontSize = 12;

            intialized = true;
        }

        GUILayout.Space(15);

        ///// Header /////
        GUILayout.BeginHorizontal();

        GUILayout.Label(myTexture, iconStyle);
        GUILayout.Label("Particle System", titleStyle);

        GUILayout.EndHorizontal();

        GUILayout.Space(30);
        GUILayout.Label("Options", labelStyle);
        //////////////////

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Particle"))
            selectedSetting = Window.particleSettings;
        else if (GUILayout.Button("System"))
            selectedSetting = Window.systemSettings;
        else if (GUILayout.Button("Physics"))
            selectedSetting = Window.physicsSettings;

        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        ///// Particle Settings /////
        if (selectedSetting == Window.particleSettings)
        {
            GUILayout.Label("Particle Settings", labelStyle);
            GUILayout.Space(10);

            // Particle Object
            GUILayout.Label("Particle Object", label2Style);
            ps.ParticleMode = (ParticleType)EditorGUILayout.EnumPopup("Particle Type", ps.ParticleMode);

            if (ps.ParticleMode == ParticleType.MeshParticle)
            {
                ps.Particle = (GameObject)EditorGUILayout.ObjectField("Particle", ps.Particle, typeof(GameObject), false);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("particleMaterial"));
            }
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

            // Lifetime in seconds
            ps.Lifetime = EditorGUILayout.FloatField("Partice lifetime", ps.Lifetime);

            EditorGUILayout.HelpBox("The lifetime of a particle in seconds.", MessageType.None);

            GUILayout.Space(10);
            // Scale of the spawn box
            GUILayout.Label("Scale", label2Style);

            if(ps.ParticleMode != ParticleType.MeshParticle)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("useScaleOverLifetime"));

            if (!ps.UseGradientScale)
                ps.Scale = EditorGUILayout.FloatField("Particale Scale", ps.Scale);
            else
            {
                ps.ScaleGradient = EditorGUILayout.Vector2Field("Scale over life", ps.ScaleGradient);
                EditorGUILayout.HelpBox("X is the start scale, Y is the maximum scale", MessageType.None);
            }

        }
        /////////////////////////////

        ///// System Settings /////
        if (selectedSetting == Window.systemSettings)
        {

            GUILayout.Label("Particle System Settings", labelStyle);
            GUILayout.Space(10);

            // General
            GUILayout.Label("General", label2Style);

            ps.MaxParticle = EditorGUILayout.IntField("Max Particles", ps.MaxParticle);

            ps.GetComponent<BoxCollider>().size = EditorGUILayout.Vector3Field("Spawnbox size", ps.GetComponent<BoxCollider>().size);

            // Loop
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loop"));

            GUILayout.Space(10);

            // Velocity
            GUILayout.Label("Velocity", label2Style);
            ps.Velocity = EditorGUILayout.Vector3Field("Particle Velocity", ps.Velocity);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useRandomVelocity"));

            if (ps.UseRandomVelocity)
            {
                ps.RandomVelocity = EditorGUILayout.Vector3Field("Random Velocity", ps.RandomVelocity);
                EditorGUILayout.HelpBox("The value you enter is the percentage of the actual velocity that will be randomized. \n" +
                                        "For example:\n" +
                                        "    When your velocity is 10 andthe random value is 100 the velocity will be between 10 & -10.\n" +
                                        "    When the velocity is 10 and the random value is 50 the velocity will be between 10 & -5 ", MessageType.None);

                GUILayout.Space(10);
            }

            GUILayout.Space(10);
            // Rotation
            GUILayout.Label("Rotation", label2Style);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useRotation"));
            EditorGUILayout.HelpBox("Do you want your particles to be rotating?\n(The use of rotation has a big impact on the games performance!)", MessageType.None);

            if (ps.Rotate && ps.ParticleMode == ParticleType.MeshParticle)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rotationSpeed"));
            }
            else if (ps.Rotate && ps.ParticleMode == ParticleType.Billboard)
            {
                Vector3 rotSpeed = ps.RotationSpeed;
                rotSpeed.z = EditorGUILayout.FloatField("Rotation Speed", ps.RotationSpeed.z);
                ps.RotationSpeed = rotSpeed;
            }
        }
        //////////////////////////

        ///// Physics Settings /////
        if (selectedSetting == Window.physicsSettings)
        {
            GUILayout.Label("Physics Settings", labelStyle);

            // Gravity
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useGravity"), true);
            if (ps.UseGravity)
            {
                ps.Gravity = EditorGUILayout.FloatField("Gravity", ps.Gravity);
                GUILayout.Space(10);
            }

            // Wind
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useWind"), true);
            if (ps.UseWind)
            {
                ps.Wind = EditorGUILayout.Vector3Field("Wind Speed", ps.Wind);
                GUILayout.Space(10);
            }
        }
        //////////////////////////////


        serializedObject.ApplyModifiedProperties();
    }
}
