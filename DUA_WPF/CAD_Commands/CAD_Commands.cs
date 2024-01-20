using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices.Filters;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Entity = Autodesk.AutoCAD.DatabaseServices.Entity;

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


    }
}
