using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;

namespace MoneyApp.Blazor.Server.Controllers
{
    public partial class GridDataController : ViewController<ListView>
    {
        public GridDataController()
        {
            InitializeComponent();

        }
        private System.ComponentModel.IContainer components = null;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View.Editor is DxGridListEditor gridListEditor)
            {
                gridListEditor.GridModel.ColumnResizeMode = DevExpress.Blazor.GridColumnResizeMode.ColumnsContainer;
                gridListEditor.GridModel.FocusedRowEnabled = true;
                gridListEditor.GridModel.VirtualScrollingEnabled = false;
                gridListEditor.GridModel.CssClass = "my-grid";
                gridListEditor.GridComponentCaptured += (_, e) => e.Grid.AutoFitColumnWidths();
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
