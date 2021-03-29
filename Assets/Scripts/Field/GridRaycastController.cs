using Runtime;

namespace Field
{
    public class GridRaycastController : IController
    {
        private GridHolder m_GridHolder;

        public GridRaycastController(GridHolder mGridHolder)
        {
            m_GridHolder = mGridHolder;
        }

        public void OnStart()
        {

        }

        public void OnStop()
        {

        }

        public void Tick()
        {
            m_GridHolder.RaycastInGrid();
        }
    }
}