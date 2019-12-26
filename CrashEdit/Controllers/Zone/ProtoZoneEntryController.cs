using Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoZoneEntryController : EntryController
    {
        public ProtoZoneEntryController(EntryChunkController entrychunkcontroller,ProtoZoneEntry zoneentry)
            : base(entrychunkcontroller,zoneentry)
        {
            ProtoZoneEntry = zoneentry;
            AddNode(new ItemController(null,zoneentry.Header));
            AddNode(new ItemController(null,zoneentry.Layout));
            foreach (OldCamera camera in zoneentry.Cameras)
            {
                AddNode(new OldCameraController(this,camera));
            }
            foreach (ProtoEntity entity in zoneentry.Entities)
            {
                AddNode(new ProtoEntityController(this,entity));
            }
            AddMenu("Export as Crash 1 ZDAT", Menu_ExportAsC1);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Prototype Zone ({0})",ProtoZoneEntry.EName);
            Node.ImageKey = "violetb";
            Node.SelectedImageKey = "violetb";
        }

        protected override Control CreateEditor()
        {
            int linkedsceneryentrycount = BitConv.FromInt32(ProtoZoneEntry.Header,0);
            ProtoSceneryEntry[] linkedsceneryentries = new ProtoSceneryEntry[linkedsceneryentrycount];
            for (int i = 0; i < linkedsceneryentrycount; i++)
            {
                linkedsceneryentries[i] = FindEID<ProtoSceneryEntry>(BitConv.FromInt32(ProtoZoneEntry.Header,4 + i * 64));
            }
            int linkedzoneentrycount = BitConv.FromInt32(ProtoZoneEntry.Header,528);
            ProtoZoneEntry[] linkedzoneentries = new ProtoZoneEntry[linkedzoneentrycount];
            for (int i = 0; i < linkedzoneentrycount; i++)
            {
                linkedzoneentries[i] = FindEID<ProtoZoneEntry>(BitConv.FromInt32(ProtoZoneEntry.Header,532 + i * 4));
            }
            return new UndockableControl(new ProtoZoneEntryViewer(ProtoZoneEntry,linkedsceneryentries,linkedzoneentries));
        }

        public ProtoZoneEntry ProtoZoneEntry { get; }

        private void Menu_ExportAsC1()
        {
            byte[] header = new byte[0x378];
            // convert header
            System.Array.Copy(ProtoZoneEntry.Header,0,header,0,0x228);
            System.Array.Copy(ProtoZoneEntry.Header,0x228,header,0x234,0xB0);
            System.Array.Copy(ProtoZoneEntry.Header,0x2EC,header,0x318,0x60);
            // convert layout
            short xmax = BitConv.FromInt16(ProtoZoneEntry.Layout,0x1E);
            short ymax = BitConv.FromInt16(ProtoZoneEntry.Layout,0x20);
            short zmax = BitConv.FromInt16(ProtoZoneEntry.Layout,0x22);
            if (ymax == 0) ymax = xmax;
            if (zmax == 0) zmax = ymax;
            byte[] layout = new byte[ProtoZoneEntry.Layout.Length];
            ProtoZoneEntry.Layout.CopyTo(layout,0);
            BitConv.ToInt16(layout,0x1E,xmax);
            BitConv.ToInt16(layout,0x20,ymax);
            BitConv.ToInt16(layout,0x22,zmax);
            // convert entities - cameras have the same format so no conversion is necessary
            List<OldEntity> entities = new List<OldEntity>();
            foreach (ProtoEntity protoentity in ProtoZoneEntry.Entities)
            {
                List<EntityPosition> pos = new List<EntityPosition>();
                int x = protoentity.StartX;
                int y = protoentity.StartY;
                int z = protoentity.StartZ;
                pos.Add(new EntityPosition((short)(x/4),(short)(y/4),(short)(z/4)));
                foreach (ProtoEntityPosition delta in protoentity.Deltas)
                {
                    x += delta.X*2;
                    y += delta.Y*2;
                    z += delta.Z*2;
                    pos.Add(new EntityPosition((short)(x/4),(short)(y/4),(short)(z/4)));
                }
                entities.Add(new OldEntity(protoentity.Garbage,protoentity.Flags,(short)(protoentity.ID+6),protoentity.ModeA,protoentity.ModeB,protoentity.ModeC,protoentity.Type,protoentity.Subtype,pos,protoentity.Nullfield1));
            }
            OldZoneEntry newzone = new OldZoneEntry(header,layout,ProtoZoneEntry.Cameras,entities,ProtoZoneEntry.EID);
            FileUtil.SaveFile(newzone.Save(),FileFilters.NSEntry,FileFilters.Any);
        }
    }
}
