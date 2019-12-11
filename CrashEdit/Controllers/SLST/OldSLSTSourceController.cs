using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldSLSTSourceController : Controller
    {
        public OldSLSTSourceController(OldSLSTEntryController oldslstentrycontroller,OldSLSTSource oldslstsource)
        {
            OldSLSTEntryController = oldslstentrycontroller;
            OldSLSTSource = oldslstsource;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Source";
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new OldSLSTSourceBox(OldSLSTSource);
        }

        public OldSLSTEntryController OldSLSTEntryController { get; }
        public OldSLSTSource OldSLSTSource { get; }
    }
}