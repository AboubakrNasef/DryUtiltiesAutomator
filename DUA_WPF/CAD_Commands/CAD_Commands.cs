using Autodesk.Aec.Geometry;
using Autodesk.Aec.Modeler;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices.Filters;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.DatabaseServices.Styles;
using Autodesk.Civil.Settings;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Autodesk.AutoCAD.DatabaseServices.AssocLoftedSurfaceActionBody;
using Entity = Autodesk.AutoCAD.DatabaseServices.Entity;
using Exception = Autodesk.AutoCAD.Runtime.Exception;
using Profile = Autodesk.Civil.DatabaseServices.Profile;

namespace DUA_WPF.CAD_Commands
{
    public partial class CMD
    {
        #region Properties
        public bool DocSavedOnce { get; set; }
        public static CivilDocument _CDoc
        {
            get
            {
                return CivilApplication.ActiveDocument;
            }
        }
        public static Document _doc
        {
            get
            {
                return Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            }
        }
        public static Database _DB
        {
            get
            {
                return Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            }
        }
        public static Editor _editor
        {
            get
            {
                return Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            }
        }
        #endregion


        public static List<LayerTableRecord> GetAllLayers()
        {
            List<LayerTableRecord> layers = new List<LayerTableRecord>();

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {
                        LayerTable layerTable = trans.GetObject(_DB.LayerTableId, OpenMode.ForRead) as LayerTable;

                        foreach (ObjectId layerId in layerTable)
                        {
                            LayerTableRecord layer = trans.GetObject(layerId, OpenMode.ForRead) as LayerTableRecord;
                            layers.Add(layer);
                        }

                        trans.Commit();
                    }
                }
                return layers;
            }
            catch (Exception)
            {

                return null;
            }

        }

        internal static List<Polyline> GetPolyLinesFromLayer(LayerTableRecord layerTableRecord)
        {
            List<Polyline> plyLines = new List<Polyline>();
            if (layerTableRecord == null)
            {
                return null;
            }

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {
                        if (layerTableRecord.Id != ObjectId.Null)
                        {
                            // Open the BlockTable for read
                            BlockTable blockTable = (BlockTable)trans.GetObject(_doc.Database.BlockTableId, OpenMode.ForRead);

                            // Open the BlockTableRecord for model space
                            BlockTableRecord modelSpace = (BlockTableRecord)trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead);


                            // Iterate through entities in model space
                            foreach (ObjectId entityId in modelSpace)
                            {
                                Entity entity = trans.GetObject(entityId, OpenMode.ForRead) as Entity;

                                // Check if the entity is a Polyline and on the target layer
                                if (entity is Polyline polyline && polyline.LayerId == layerTableRecord.Id)
                                {
                                    plyLines.Add(polyline);
                                }
                            }
                        }


                        trans.Commit();
                    }
                }
                return plyLines;
            }

            catch (Exception)
            {

                return null;
            }
        }

        internal static List<Assembly> GetAllAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {

                        foreach (ObjectId objId in _CDoc.AssemblyCollection)
                        {
                            Assembly assembly = trans.GetObject(objId, OpenMode.ForRead) as Assembly;
                            assemblies.Add(assembly);
                        }
                        trans.Commit();
                    }
                }
                return assemblies;
            }
            catch (Exception)
            {

                return null;
            }
        }

        internal static List<ProfileViewStyle> GetAllProfileViewStyles()
        {
            List<ProfileViewStyle> ProfileStyles = new List<ProfileViewStyle>();

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {
                        ProfileViewStyleCollection ProfileStylesCol = _CDoc.Styles.ProfileViewStyles;

                        // Loop through each alignment style
                        foreach (ObjectId styleId in ProfileStylesCol)
                        {
                            ProfileViewStyle style = trans.GetObject(styleId, OpenMode.ForRead) as ProfileViewStyle;

                            if (style != null)
                            {
                                ProfileStyles.Add(style);
                            }
                        }

                        trans.Commit();
                    }
                }
                return ProfileStyles;
            }
            catch (Exception)
            {

                return null;
            }
        }

        internal static List<ProfileViewBandSetStyle> GetAllProfileViewBandSets()
        {
            List<ProfileViewBandSetStyle> ProfileBandStyles = new List<ProfileViewBandSetStyle>();

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {
                        // Get the AlignmentStyleCollection
                        ProfileViewBandSetStyleCollection ProfileBandsCol = _CDoc.Styles.ProfileViewBandSetStyles;

                        // Loop through each alignment style
                        foreach (ObjectId styleId in ProfileBandsCol)
                        {
                            ProfileViewBandSetStyle style = trans.GetObject(styleId, OpenMode.ForRead) as ProfileViewBandSetStyle;

                            if (style != null)
                            {
                                ProfileBandStyles.Add(style);
                            }
                        }

                        trans.Commit();
                    }
                }
                return ProfileBandStyles;
            }
            catch (Exception)
            {

                return null;
            }
        }

        internal static List<TinSurface> GetAllTinSurfaces()
        {
            List<TinSurface> Surfaces = new List<TinSurface>();

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {


                        var surfacesIDS = _CDoc.GetSurfaceIds();
                        foreach (ObjectId item in surfacesIDS)
                        {
                            TinSurface surface = trans.GetObject(item, OpenMode.ForRead) as TinSurface;
                            Surfaces.Add(surface);
                        }

                        trans.Commit();
                    }
                }
                return Surfaces;
            }
            catch (Exception)
            {

                return null;
            }
        }

        internal static List<ProfileLabelSetStyle> GetAllProfileLabelSets()
        {
            List<ProfileLabelSetStyle> ProfileLabelSetStyles = new List<ProfileLabelSetStyle>();

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {

                        // Get the AlignmentLabelStyleCollection
                        ProfileLabelSetStyleCollection labelStyles = _CDoc.Styles.LabelSetStyles.ProfileLabelSetStyles;

                        // Loop through each label style
                        foreach (ObjectId styleId in labelStyles)
                        {
                            ProfileLabelSetStyle style = trans.GetObject(styleId, OpenMode.ForRead) as ProfileLabelSetStyle;

                            if (style != null)
                            {
                                ProfileLabelSetStyles.Add(style);
                            }
                        }
                        trans.Commit();
                    }
                }
                return ProfileLabelSetStyles;
            }
            catch (Exception)
            {

                return null;
            }
        }

        internal static List<ProfileStyle> GetAllProfileStyle()
        {
            List<ProfileStyle> ProfileStyles = new List<ProfileStyle>();

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {
                        // Get the AlignmentStyleCollection
                        ProfileStyleCollection ProfileStylesCol = _CDoc.Styles.ProfileStyles;

                        // Loop through each alignment style
                        foreach (ObjectId styleId in ProfileStylesCol)
                        {
                            ProfileStyle style = trans.GetObject(styleId, OpenMode.ForRead) as ProfileStyle;

                            if (style != null)
                            {
                                ProfileStyles.Add(style);
                            }
                        }

                        trans.Commit();
                    }
                }
                return ProfileStyles;
            }
            catch (Exception)
            {

                return null;
            }
        }

        internal static List<AlignmentLabelSetStyle> GetAllAlignmentLabelSet()
        {
            List<AlignmentLabelSetStyle> alignmentLabels = new List<AlignmentLabelSetStyle>();

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {

                        // Get the AlignmentLabelStyleCollection
                        AlignmentLabelSetStyleCollection labelStyles = _CDoc.Styles.LabelSetStyles.AlignmentLabelSetStyles;

                        // Loop through each label style
                        foreach (ObjectId styleId in labelStyles)
                        {
                            AlignmentLabelSetStyle style = trans.GetObject(styleId, OpenMode.ForRead) as AlignmentLabelSetStyle;

                            if (style != null)
                            {
                                alignmentLabels.Add(style);
                            }
                        }
                        trans.Commit();
                    }
                }
                return alignmentLabels;
            }
            catch (Exception)
            {

                return null;
            }
        }

        internal static List<AlignmentStyle> GetAllAlignmentStyles()
        {
            List<AlignmentStyle> alignmentStyles = new List<AlignmentStyle>();

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {
                        // Get the AlignmentStyleCollection
                        AlignmentStyleCollection alignmentStylesCol = _CDoc.Styles.AlignmentStyles;

                        // Loop through each alignment style
                        foreach (ObjectId styleId in alignmentStylesCol)
                        {
                            AlignmentStyle style = trans.GetObject(styleId, OpenMode.ForRead) as AlignmentStyle;

                            if (style != null)
                            {
                                alignmentStyles.Add(style);
                            }
                        }

                        trans.Commit();
                    }
                }
                return alignmentStyles;
            }
            catch (Exception)
            {

                return null;
            }
        }

        internal static List<CorridorStyle> GetAllCorridorStyles()
        {
            List<CorridorStyle> CorridorStyles = new List<CorridorStyle>();

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {
                        // Get the AlignmentStyleCollection
                        CorridorStyleCollection alignmentStylesCol = _CDoc.Styles.CorridorStyles;

                        // Loop through each alignment style
                        foreach (ObjectId styleId in alignmentStylesCol)
                        {
                            CorridorStyle style = trans.GetObject(styleId, OpenMode.ForRead) as CorridorStyle;

                            if (style != null)
                            {
                                CorridorStyles.Add(style);
                            }
                        }

                        trans.Commit();
                    }
                }
                return CorridorStyles;
            }
            catch (Exception)
            {

                return null;
            }
        }

        internal static List<Polyline> GetPolylinesFromLayer(LayerTableRecord layer)
        {
            List<Polyline> polylines = new List<Polyline>();
            TypedValue[] tvs = new TypedValue[] {
            new TypedValue(Convert.ToInt32(DxfCode.Operator), "<and"),
            new TypedValue(Convert.ToInt32(DxfCode.LayerName), layer.Name),
            new TypedValue(Convert.ToInt32(DxfCode.Operator), "<or"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "POLYLINE"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "LWPOLYLINE"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "POLYLINE2D"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "POLYLINE3d"),
            new TypedValue(Convert.ToInt32(DxfCode.Operator), "or>"),
            new TypedValue(Convert.ToInt32(DxfCode.Operator), "and>")};
            SelectionFilter sf = new SelectionFilter(tvs);
            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {
                        PromptSelectionResult psr = _editor.SelectAll(sf);

                        if (psr.Status == PromptStatus.OK)
                        {


                            foreach (ObjectId id in psr.Value.GetObjectIds())
                            {
                                var ent = trans.GetObject(id, OpenMode.ForRead) as Polyline;
                                if (ent is Polyline && ent.LayerId == layer.Id)
                                {
                                    polylines.Add(ent);
                                }



                            }


                        }

                        trans.Commit();
                    }
                }
                return polylines;
            }
            catch (Exception)
            {

                return null;
            }
        }

        internal static List<Polyline> SelectPolyLines()
        {
            List<Polyline> polylines = new List<Polyline>();
            TypedValue[] tvs = new TypedValue[] {
            new TypedValue(Convert.ToInt32(DxfCode.Operator), "<and"),

            new TypedValue(Convert.ToInt32(DxfCode.Operator), "<or"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "POLYLINE"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "LWPOLYLINE"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "POLYLINE2D"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "POLYLINE3d"),
            new TypedValue(Convert.ToInt32(DxfCode.Operator), "or>"),
            new TypedValue(Convert.ToInt32(DxfCode.Operator), "and>")};
            SelectionFilter sf = new SelectionFilter(tvs);

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {
                        PromptSelectionResult psr = _editor.GetSelection(sf);

                        if (psr.Status == PromptStatus.OK)
                        {


                            foreach (ObjectId id in psr.Value.GetObjectIds())
                            {
                                var ent = trans.GetObject(id, OpenMode.ForRead) as Polyline;
                                if (ent is Polyline)
                                {
                                    polylines.Add(ent);
                                }



                            }


                        }

                        trans.Commit();
                    }
                }
                return polylines;
            }
            catch (Exception)
            {

                return null;
            }
        }


        internal static Alignment CreateAlignmentFromPolyLine(Polyline pline, string AlignmentName, AlignmentStyle alignmentStyle, AlignmentLabelSetStyle alignmentLabelSets)
        {
            Alignment alignment = null;

            try
            {


                using (DocumentLock dockLock = _doc.LockDocument())
                {
                    using (Transaction trans = _DB.TransactionManager.StartTransaction())
                    {



                        // create some polyline options for creating the new alignment
                        PolylineOptions plops = new PolylineOptions();
                        plops.AddCurvesBetweenTangents = true;
                        plops.EraseExistingEntities = true;
                        plops.PlineId = pline.ObjectId;

                        SettingsCmdCreateAlignmentEntities settingsCmdCreateAlignmentEntities = _CDoc.Settings.GetSettings<SettingsCmdCreateAlignmentEntities>();
                        double tempRadiusHolder = settingsCmdCreateAlignmentEntities.CreateFromEntities.Radius.Value;
                        settingsCmdCreateAlignmentEntities.CreateFromEntities.Radius.Value = 0.1;
                        SettingsObjectLayer objectLayer = _CDoc.Settings.DrawingSettings.ObjectLayerSettings.GetObjectLayerSetting(SettingsObjectLayerType.Alignment);


                        ObjectId AlignmentID = Alignment.Create(
                        _CDoc,
                        plops,
                        AlignmentName,
                        ObjectId.Null, objectLayer.LayerId,
                        alignmentStyle.Id,
                        alignmentLabelSets.Id);
                        settingsCmdCreateAlignmentEntities.CreateFromEntities.Radius.Value = tempRadiusHolder;
                        alignment = trans.GetObject(AlignmentID, OpenMode.ForRead) as Alignment;
                        trans.Commit();
                    }
                }
                return alignment;
            }
            catch (Exception)
            {

                return null;
            }
        }




        internal static Profile CreateSurfaceProfile(Alignment alignment, TinSurface surface, ProfileStyle profileStyle, ProfileLabelSetStyle profileLabelSet)
        {
            Profile profile = null;
            using (DocumentLock dockLock = _doc.LockDocument())
            {
                using (Transaction tr = _DB.TransactionManager.StartTransaction())
                {
                    try
                    {




                        // Create a profile from the alignment and surface
                        SettingsObjectLayer objectLayer = _CDoc.Settings.DrawingSettings.ObjectLayerSettings.GetObjectLayerSetting(SettingsObjectLayerType.Profile);

                        ObjectId profileId = Profile.CreateFromSurface(surface.Name + "-Profile", alignment.ObjectId, surface.ObjectId, objectLayer.LayerId, profileStyle.ObjectId, profileLabelSet.ObjectId);
                        profile = tr.GetObject(profileId, OpenMode.ForRead) as Profile;






                        tr.Commit();

                    }
                    catch (System.Exception ex)
                    {
                    }

                    return profile;
                }
            }
        }

        internal static Profile CreateOffsetProfile(Alignment alignment, Profile profile, string profileName, double offset, ProfileStyle profileStyle, ProfileLabelSetStyle profileLabelSet)
        {


            Profile offseted_profile = null;
            using (DocumentLock dockLock = _doc.LockDocument())
            {
                using (Transaction tr = _DB.TransactionManager.StartTransaction())
                {
                    try
                    {
                        // Create a profile from the alignment and surface
                        SettingsObjectLayer objectLayer = _CDoc.Settings.DrawingSettings.ObjectLayerSettings.GetObjectLayerSetting(SettingsObjectLayerType.Profile);

                        var profileOffsetID = Profile.CreateByLayout(profileName, alignment.ObjectId, objectLayer.LayerId, profileStyle.ObjectId, profileLabelSet.ObjectId);
                        offseted_profile = tr.GetObject(profileOffsetID, OpenMode.ForRead) as Profile;





                        foreach (ProfileEntity entity in profile.Entities)
                        {

                            switch (entity.EntityType)
                            {
                                case ProfileEntityType.Tangent:
                                    var p1 = new Point2d(entity.StartStation, entity.StartElevation + offset);
                                    var p2 = new Point2d(entity.EndStation, entity.EndElevation + offset);
                                    offseted_profile.Entities.AddFixedTangent(p1, p2);

                                    break;

                                default:
                                    break;
                            }

                        }

                        tr.Commit();
                    }
                    catch (System.Exception ex)
                    {
                    }

                    return offseted_profile;
                }
            }






        }


        internal static Point3d GetPoint()
        {

            return _editor.GetPoint("Select Top Left Point For Profile Views").Value;
        }

       
        public static ProfileView CreateProfileView(Alignment alignment, Point3d insertionPoint, ProfileViewBandSetStyle profileViewBandStyle, ProfileViewStyle profileViewStyle)
        {
            ProfileView profileView = null;

            using (DocumentLock dockLock = _doc.LockDocument())
            {
                using (Transaction tr = _DB.TransactionManager.StartTransaction())
                {
                    try
                    {



                        // alignment


                        // Create a profile from the alignment and surface
                        SettingsObjectLayer objectLayer = _CDoc.Settings.DrawingSettings.ObjectLayerSettings.GetObjectLayerSetting(SettingsObjectLayerType.Profile);



                        var profileViewID = ProfileView.Create(alignment.Id, insertionPoint, alignment.Name + "ProfileView", profileViewBandStyle.Id, profileViewStyle.Id);
                        profileView = tr.GetObject(profileViewID, OpenMode.ForRead) as ProfileView;






                        tr.Commit();



                    }
                    catch (System.Exception ex)
                    {

                    }

                }
            }

            return profileView;
        }


        public static Corridor CreateCorridor(Alignment alignment, Profile prof, Assembly assembly)
        {

            Corridor corridor = null;
            using (DocumentLock dockLock = _doc.LockDocument())
            {
                using (Transaction tr = _DB.TransactionManager.StartTransaction())
                {
                    try
                    {

                        

              

                      
                        // Create a profile from the alignment and surface
                        SettingsObjectLayer objectLayer = _CDoc.Settings.DrawingSettings.ObjectLayerSettings.GetObjectLayerSetting(SettingsObjectLayerType.Profile);


                        // Create a new Corridor
                   
                        ObjectId newCorridorId = _CDoc.CorridorCollection.Add(alignment.Name +"-Corridor", "BaseLine", alignment.Id, prof.Id, "Region", assembly.Id);

                         corridor = tr.GetObject(newCorridorId, OpenMode.ForWrite) as Corridor;


                        corridor.Rebuild();

                  
                        tr.Commit();



                    }
                    catch (System.Exception ex)
                    {
                       
                    }
                }
            }

           return corridor;
        }



    }
}
