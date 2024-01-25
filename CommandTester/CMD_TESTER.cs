using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.DatabaseServices.Styles;
using Autodesk.Civil.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: CommandClass(typeof(CommandTester.CMD_TESTER))]
namespace CommandTester
{
    public class CMD_TESTER
    {


        [CommandMethod("za_CreateAlignmentFrom_PolyLine")]

        public static void CreateAlignmentFromPolyline()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            CivilDocument civDoc = CivilApplication.ActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            // Ask the user to select a polyline
            PromptEntityResult result = ed.GetEntity("Select a polyline:");
            if (result.Status != PromptStatus.OK || result.ObjectId.IsNull)
            {
                ed.WriteMessage("\nError in selecting polyline.");
                return;
            }

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // Open the polyline
                    Polyline pline = tr.GetObject(result.ObjectId, OpenMode.ForRead) as Polyline;

                    if (pline == null)
                    {
                        ed.WriteMessage("\nError in opening polyline.");
                        return;
                    }


                    // create some polyline options for creating the new alignment
                    PolylineOptions plops = new PolylineOptions();
                    plops.AddCurvesBetweenTangents = true;
                    plops.EraseExistingEntities = true;
                    plops.PlineId = pline.ObjectId;

                    SettingsCmdCreateAlignmentEntities settingsCmdCreateAlignmentEntities = civDoc.Settings.GetSettings<SettingsCmdCreateAlignmentEntities>();
                    double tempRadiusHolder = settingsCmdCreateAlignmentEntities.CreateFromEntities.Radius.Value;
                    settingsCmdCreateAlignmentEntities.CreateFromEntities.Radius.Value = 0.1;
                    SettingsObjectLayer objectLayer = civDoc.Settings.DrawingSettings.ObjectLayerSettings.GetObjectLayerSetting(SettingsObjectLayerType.Alignment);

                    var AlStyles = ListAlignmentStyles();
                    var labelStyles = ListAlignmentLabelStyles();
                    ObjectId AlignmentID = Alignment.Create(
                         civDoc,
                         plops,
                         "New Alignment",
                         ObjectId.Null, objectLayer.LayerId,
                   AlStyles[0].Id,
                        labelStyles[1].Id);
                    settingsCmdCreateAlignmentEntities.CreateFromEntities.Radius.Value = tempRadiusHolder;
                    tr.Commit();


                    ed.WriteMessage("\nAlignment created successfully.");

                    return;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\nError: " + ex.Message);
                    return;
                }
            }
        }



        public static List<AlignmentStyle> ListAlignmentStyles()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                List<AlignmentStyle> alignmentStyles = new List<AlignmentStyle>();
                try
                {
                    CivilDocument civilDoc = CivilApplication.ActiveDocument;

                    if (civilDoc == null)
                    {
                        ed.WriteMessage("\nCivil 3D document not found.");
                        return null;
                    }

                    // Get the AlignmentStyleCollection
                    AlignmentStyleCollection alignmentStylesCol = civilDoc.Styles.AlignmentStyles;

                    // Loop through each alignment style
                    foreach (ObjectId styleId in alignmentStylesCol)
                    {
                        AlignmentStyle style = tr.GetObject(styleId, OpenMode.ForRead) as AlignmentStyle;

                        if (style != null)
                        {
                            alignmentStyles.Add(style);
                        }
                    }
                    return alignmentStyles;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\nError: " + ex.Message);
                    return null;
                }
            }
        }


        public static List<AlignmentLabelSetStyle> ListAlignmentLabelStyles()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                List<AlignmentLabelSetStyle> alignmentLabels = new List<AlignmentLabelSetStyle>();
                try
                {
                    CivilDocument civilDoc = CivilApplication.ActiveDocument;

                    if (civilDoc == null)
                    {
                        ed.WriteMessage("\nCivil 3D document not found.");
                        return null;
                    }

                    // Get the AlignmentLabelStyleCollection
                    AlignmentLabelSetStyleCollection labelStyles = civilDoc.Styles.LabelSetStyles.AlignmentLabelSetStyles;

                    // Loop through each label style
                    foreach (ObjectId styleId in labelStyles)
                    {
                        AlignmentLabelSetStyle style = tr.GetObject(styleId, OpenMode.ForRead) as AlignmentLabelSetStyle;

                        if (style != null)
                        {
                            alignmentLabels.Add(style);
                        }
                    }

                    return alignmentLabels;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\nError: " + ex.Message);
                    return null;
                }
            }
        }



        [CommandMethod("za_CreateSurfaceProfile")]
        public static void CreateSurfaceProfile()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            // Prompt the user to select an alignment
            PromptEntityOptions promptOptions = new PromptEntityOptions("\nSelect an alignment:");
            promptOptions.SetRejectMessage("\nInvalid selection. Please select an alignment.");
            promptOptions.AddAllowedClass(typeof(Alignment), false);

            PromptEntityResult result = ed.GetEntity(promptOptions);

            if (result.Status != PromptStatus.OK || result.ObjectId.IsNull)
            {
                ed.WriteMessage("\nError in selecting Alignment.");
                return;
            }
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {

                    CivilDocument civilDoc = CivilApplication.ActiveDocument;

                    if (civilDoc == null)
                    {
                        ed.WriteMessage("\nCivil 3D document not found.");
                        return;
                    }

                    // alignment
                    Alignment alignment = tr.GetObject(result.ObjectId, OpenMode.ForRead) as Alignment;
                    // Create or select a surface
                    TinSurface surface = SelectSurface(civilDoc, tr);
                    var styles = ListProfileStyles();
                    var labelSets = ListProfileLabelStyles();
                    // Create a profile from the alignment and surface
                    SettingsObjectLayer objectLayer = civilDoc.Settings.DrawingSettings.ObjectLayerSettings.GetObjectLayerSetting(SettingsObjectLayerType.Profile);

                    ObjectId profileId = Profile.CreateFromSurface("Profile1", alignment.ObjectId, surface.ObjectId, objectLayer.LayerId, styles[0].ObjectId, labelSets[0].ObjectId);
                    Profile profile = tr.GetObject(profileId, OpenMode.ForRead) as Profile;


                    var profileOffsetID = Profile.CreateByLayout("Profile2", alignment.ObjectId, objectLayer.LayerId, styles[1].ObjectId, labelSets[1].ObjectId);
                    Profile profileOffset = tr.GetObject(profileOffsetID, OpenMode.ForRead) as Profile;

                    //Curve3d offsetedCurve = curve.Clone() as Curve3d;
                    //offsetedCurve.TranslateBy(new Vector3d(0, 0, 0.6));
                    CreateOffsetProfile(profile, profileOffset, -0.6);



                    ed.WriteMessage("\nSurface profile created successfully.");
                    tr.Commit();

                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\nError: " + ex.Message);
                }
            }
        }

        private static void CreateOffsetProfile(Profile profile, Profile profileOffset, double offset)
        {


            foreach (ProfileEntity entity in profile.Entities)
            {

                switch (entity.EntityType)
                {
                    case ProfileEntityType.Tangent:
                        var p1 = new Point2d(entity.StartStation, entity.StartElevation + offset);
                        var p2 = new Point2d(entity.EndStation, entity.EndElevation + offset);
                        profileOffset.Entities.AddFixedTangent(p1, p2);

                        break;

                    default:
                        break;
                }

            }



        }

        public static List<ProfileStyle> ListProfileStyles()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                List<ProfileStyle> ProfileStyles = new List<ProfileStyle>();
                try
                {
                    CivilDocument civilDoc = CivilApplication.ActiveDocument;

                    if (civilDoc == null)
                    {
                        ed.WriteMessage("\nCivil 3D document not found.");
                        return null;
                    }

                    // Get the AlignmentStyleCollection
                    ProfileStyleCollection ProfileStylesCol = civilDoc.Styles.ProfileStyles;

                    // Loop through each alignment style
                    foreach (ObjectId styleId in ProfileStylesCol)
                    {
                        ProfileStyle style = tr.GetObject(styleId, OpenMode.ForRead) as ProfileStyle;

                        if (style != null)
                        {
                            ProfileStyles.Add(style);
                        }
                    }
                    return ProfileStyles;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\nError: " + ex.Message);
                    return null;
                }
            }
        }


        public static List<ProfileLabelSetStyle> ListProfileLabelStyles()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                List<ProfileLabelSetStyle> ProfileLabelSetStyles = new List<ProfileLabelSetStyle>();
                try
                {
                    CivilDocument civilDoc = CivilApplication.ActiveDocument;

                    if (civilDoc == null)
                    {
                        ed.WriteMessage("\nCivil 3D document not found.");
                        return null;
                    }

                    // Get the AlignmentLabelStyleCollection
                    ProfileLabelSetStyleCollection labelStyles = civilDoc.Styles.LabelSetStyles.ProfileLabelSetStyles;

                    // Loop through each label style
                    foreach (ObjectId styleId in labelStyles)
                    {
                        ProfileLabelSetStyle style = tr.GetObject(styleId, OpenMode.ForRead) as ProfileLabelSetStyle;

                        if (style != null)
                        {
                            ProfileLabelSetStyles.Add(style);
                        }
                    }

                    return ProfileLabelSetStyles;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\nError: " + ex.Message);
                    return null;
                }
            }
        }


        private static TinSurface SelectSurface(CivilDocument civilDoc, Transaction tr)
        {
            TinSurface surface;
            var surfaces = civilDoc.GetSurfaceIds();

            // Open the existing surface
            surface = tr.GetObject(surfaces[0], OpenMode.ForWrite) as TinSurface;


            return surface;
        }






        [CommandMethod("za_CreateProfileView")]
        public static void CreateProfileView()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            // Prompt the user to select an alignment
            PromptEntityOptions promptOptions = new PromptEntityOptions("\nSelect an alignment:");
            promptOptions.SetRejectMessage("\nInvalid selection. Please select an alignment.");
            promptOptions.AddAllowedClass(typeof(Alignment), false);

            PromptEntityResult result = ed.GetEntity(promptOptions);

            if (result.Status != PromptStatus.OK || result.ObjectId.IsNull)
            {
                ed.WriteMessage("\nError in selecting Alignment.");
                return;
            }
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {

                    CivilDocument civilDoc = CivilApplication.ActiveDocument;

                    if (civilDoc == null)
                    {
                        ed.WriteMessage("\nCivil 3D document not found.");
                        return;
                    }

                    // alignment
                    Alignment alignment = tr.GetObject(result.ObjectId, OpenMode.ForRead) as Alignment;

                    var styles = ListProfileViewStyles();
                    var bandSetStyles = ListProfileViewBandSets();
                    // Create a profile from the alignment and surface
                    SettingsObjectLayer objectLayer = civilDoc.Settings.DrawingSettings.ObjectLayerSettings.GetObjectLayerSetting(SettingsObjectLayerType.Profile);

                    var pointResult = ed.GetPoint("Select Point Of Insertion");
                    if (pointResult.Status == PromptStatus.OK)
                    {
                        Point3d insertionPoint = pointResult.Value;
                        var profileViewID = ProfileView.Create(alignment.Id, insertionPoint, "ProfileView", bandSetStyles[2].Id, styles[7].Id);
                        ProfileView profileView = tr.GetObject(result.ObjectId, OpenMode.ForRead) as ProfileView;
                       




                        ed.WriteMessage("\n profile view created successfully.");
                        tr.Commit();
                    }


                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\nError: " + ex.Message);
                }
            }
        }


        public static List<ProfileViewStyle> ListProfileViewStyles()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                List<ProfileViewStyle> ProfileStyles = new List<ProfileViewStyle>();
                try
                {
                    CivilDocument civilDoc = CivilApplication.ActiveDocument;

                    if (civilDoc == null)
                    {
                        ed.WriteMessage("\nCivil 3D document not found.");
                        return null;
                    }

                    // Get the AlignmentStyleCollection
                    ProfileViewStyleCollection ProfileStylesCol = civilDoc.Styles.ProfileViewStyles;

                    // Loop through each alignment style
                    foreach (ObjectId styleId in ProfileStylesCol)
                    {
                        ProfileViewStyle style = tr.GetObject(styleId, OpenMode.ForRead) as ProfileViewStyle;

                        if (style != null)
                        {
                            ProfileStyles.Add(style);
                        }
                    }
                    return ProfileStyles;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\nError: " + ex.Message);
                    return null;
                }
            }
        }

        public static List<ProfileViewBandSetStyle> ListProfileViewBandSets()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                List<ProfileViewBandSetStyle> ProfileStyles = new List<ProfileViewBandSetStyle>();
                try
                {
                    CivilDocument civilDoc = CivilApplication.ActiveDocument;

                    if (civilDoc == null)
                    {
                        ed.WriteMessage("\nCivil 3D document not found.");
                        return null;
                    }

                    // Get the AlignmentStyleCollection
                    ProfileViewBandSetStyleCollection ProfileBandsCol = civilDoc.Styles.ProfileViewBandSetStyles;

                    // Loop through each alignment style
                    foreach (ObjectId styleId in ProfileBandsCol)
                    {
                        ProfileViewBandSetStyle style = tr.GetObject(styleId, OpenMode.ForRead) as ProfileViewBandSetStyle;

                        if (style != null)
                        {
                            ProfileStyles.Add(style);
                        }
                    }
                    return ProfileStyles;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\nError: " + ex.Message);
                    return null;
                }
            }
        }

        //Corridors

        [CommandMethod("za_CreateCorridor")]
        public static void CreateCorridor()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            // Prompt the user to select an alignment
            PromptEntityOptions promptOptions = new PromptEntityOptions("\nSelect an alignment:");
            promptOptions.SetRejectMessage("\nInvalid selection. Please select an alignment.");
            promptOptions.AddAllowedClass(typeof(Alignment), false);

            PromptEntityResult result = ed.GetEntity(promptOptions);

            if (result.Status != PromptStatus.OK || result.ObjectId.IsNull)
            {
                ed.WriteMessage("\nError in selecting Alignment.");
                return;
            }
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {

                    CivilDocument civilDoc = CivilApplication.ActiveDocument;

                    if (civilDoc == null)
                    {
                        ed.WriteMessage("\nCivil 3D document not found.");
                        return;
                    }

                    // alignment
                    Alignment alignment = tr.GetObject(result.ObjectId, OpenMode.ForRead) as Alignment;
                    List<Profile> profiles = GetProfilesFromAlignment(alignment, tr);

                    Profile prof = profiles.First(x => x.ProfileType == ProfileType.FG);

                    // Create a profile from the alignment and surface
                    SettingsObjectLayer objectLayer = civilDoc.Settings.DrawingSettings.ObjectLayerSettings.GetObjectLayerSetting(SettingsObjectLayerType.Profile);


                    // Create a new Corridor
                    var assembles = ListAssemblies();
                    ObjectId newCorridorId = civilDoc.CorridorCollection.Add("Corridor1", "BaseLine1", alignment.Id, prof.Id, "Region1", assembles[0].Id);

                    Corridor corridor = tr.GetObject(newCorridorId, OpenMode.ForWrite) as Corridor;


                    corridor.Rebuild();

                    ed.WriteMessage("\n profile view created successfully.");
                    tr.Commit();



                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\nError: " + ex.Message);
                }
            }
        }

        private static List<Profile> GetProfilesFromAlignment(Alignment alignment, Transaction tr)
        {
            var profiles = new List<Profile>();
            var profileIds = alignment.GetProfileIds();

            foreach (ObjectId profileId in profileIds)
            {
                var profile = tr.GetObject(profileId, OpenMode.ForRead) as Profile;

                profiles.Add(profile);
            }

            return profiles;
        }

        public static List<Assembly> ListAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();
            CivilDocument doc = CivilApplication.ActiveDocument;
            Editor ed =
            Application.DocumentManager.MdiActiveDocument.Editor;

            using (Transaction ts =
            Application.DocumentManager.MdiActiveDocument.
            Database.TransactionManager.StartTransaction())
            {
                foreach (ObjectId objId in doc.AssemblyCollection)
                {
                    Assembly myCorridor = ts.GetObject(objId, OpenMode.ForRead) as Assembly;
                    assemblies.Add(myCorridor);
                }
            }


            return assemblies;
        }


    }
}

