using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.DatabaseServices.Styles;
using Autodesk.Civil.DatabaseServices;

using System.Collections.Generic;

using DUA_WPF.CAD_Commands;
using Autodesk.AutoCAD.Geometry;

namespace DUA_WPF.Services
{
    public interface ICADService
    {
        List<LayerTableRecord> Layers { get; }
        List<Assembly> Assemblies { get; }
        List<ProfileViewStyle> ProfileViewStyles { get; }
        List<ProfileViewBandSetStyle> ProfileViewBandSetStyles { get; }
        List<TinSurface> TinSurfaces { get; }
        List<ProfileLabelSetStyle> ProfileLabelSetStyles { get; }
        List<ProfileStyle> ProfileStyles { get; }
        List<AlignmentLabelSetStyle> AlignmentLabelSetItems { get; }
        List<AlignmentStyle> AlignmentStyles { get; }
        List<CorridorStyle> CorridorStyles { get;  }

        List<Polyline> GetPolylinesFromLayer(LayerTableRecord layer);
        List<Polyline> GetSelectedPolylines( );

        Alignment CreateAlignmentFromPolyLine(Polyline pline, string AlignmentName, AlignmentStyle alignmentStyle, AlignmentLabelSetStyle alignmentLabelSets);

        Profile CreateSurfaceProfile(Alignment alignment, TinSurface surface, ProfileStyle profileStyle, ProfileLabelSetStyle profileLabelSet);
        Profile CreateOffsetProfile(Alignment alignment, Profile profile, string profileName, double offset, ProfileStyle profileStyle, ProfileLabelSetStyle profileLabelSet);
        Point3d GetPoint();
        ProfileView CreateProfileView(Alignment alignment, Point3d insertionPoint, ProfileViewBandSetStyle profileViewBandStyle, ProfileViewStyle profileViewStyle);
        Corridor CreateCorridor(Alignment alignment, Profile prof, Assembly assembly);

    }
    public class CADService : ICADService
    {
        private List<LayerTableRecord> _layers;
        private List<Assembly> _assemblies;
        private List<CorridorStyle> _corridorStyles;
        private List<ProfileViewStyle> _profileViewStyles;
        private List<ProfileViewBandSetStyle> _profileViewBandSetStyles;
        private List<TinSurface> _tinSurfaces;
        private List<ProfileLabelSetStyle> _profileLabelSetStyles;
        private List<ProfileStyle> _profileStyles;
        private List<AlignmentLabelSetStyle> _alignmentLabelSetItems;
        private List<AlignmentStyle> _alignmentStyles;
        public List<LayerTableRecord> Layers => _layers;


        public List<Assembly> Assemblies => _assemblies;

        public List<ProfileViewStyle> ProfileViewStyles => _profileViewStyles;

        public List<ProfileViewBandSetStyle> ProfileViewBandSetStyles => _profileViewBandSetStyles;

        public List<TinSurface> TinSurfaces => _tinSurfaces;

        public List<ProfileLabelSetStyle> ProfileLabelSetStyles => _profileLabelSetStyles;

        public List<ProfileStyle> ProfileStyles => _profileStyles;

        public List<AlignmentLabelSetStyle> AlignmentLabelSetItems => _alignmentLabelSetItems;

        public List<AlignmentStyle> AlignmentStyles => _alignmentStyles;

        public List<CorridorStyle> CorridorStyles => _corridorStyles;

        public CADService()
        {
            _layers = CMD.GetAllLayers();
            _assemblies = CMD.GetAllAssemblies();
            _profileViewStyles = CMD.GetAllProfileViewStyles();
            _profileViewBandSetStyles = CMD.GetAllProfileViewBandSets();
            _tinSurfaces = CMD.GetAllTinSurfaces();
            _profileLabelSetStyles = CMD.GetAllProfileLabelSets();
            _profileStyles = CMD.GetAllProfileStyle();
            _alignmentLabelSetItems = CMD.GetAllAlignmentLabelSet();
            _alignmentStyles = CMD.GetAllAlignmentStyles();
            _corridorStyles = CMD.GetAllCorridorStyles();
        }

        public List<Polyline> GetPolylinesFromLayer(LayerTableRecord layer)
        {
            return CMD.GetPolyLinesFromLayer(layer);
        }

        public List<Polyline> GetSelectedPolylines()
        {
            return CMD.SelectPolyLines();
        }

        public Alignment CreateAlignmentFromPolyLine(Polyline pline, string AlignmentName, AlignmentStyle alignmentStyle, AlignmentLabelSetStyle alignmentLabelSets)
        {
         return   CMD.CreateAlignmentFromPolyLine(pline, AlignmentName, alignmentStyle, alignmentLabelSets);
        }

        public Profile CreateSurfaceProfile(Alignment alignment, TinSurface surface, ProfileStyle profileStyle, ProfileLabelSetStyle profileLabelSet)
        {
            return CMD.CreateSurfaceProfile(alignment, surface, profileStyle, profileLabelSet);
        }

        public Profile CreateOffsetProfile(Alignment alignment, Profile profile, string profileName, double offset, ProfileStyle profileStyle, ProfileLabelSetStyle profileLabelSet)
        {
           return CMD.CreateOffsetProfile(alignment,profile,profileName,offset,profileStyle,profileLabelSet);   
        }

        public Point3d GetPoint()
        {
            return CMD.GetPoint();
        }

        public ProfileView CreateProfileView(Alignment alignment, Point3d insertionPoint, ProfileViewBandSetStyle profileViewBandStyle, ProfileViewStyle profileViewStyle)
        {
           return CMD.CreateProfileView(alignment,insertionPoint,profileViewBandStyle,profileViewStyle);
        }

        public Corridor CreateCorridor(Alignment alignment, Profile prof, Assembly assembly)
        {
         return   CMD.CreateCorridor(alignment, prof, assembly);
        }
    }


}
