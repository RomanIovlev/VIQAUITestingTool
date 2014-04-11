namespace VIQA
{
    public class VIAction<T> where T : class
    {
        public T DefaultAction;
        public VIAction(T defaultAction) { DefaultAction = defaultAction; }

        private T _getElementLabelFunc;
        public T Action
        {
            set { _getElementLabelFunc = value; }
            get { return _getElementLabelFunc ?? DefaultAction; }
        }
    }
}
