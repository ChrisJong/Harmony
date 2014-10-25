namespace Input {
    public interface ITouchable {
        void OnTouchBegan();
        void OnTouchEnded();
        void OnTouchMoved();
        void OnTouchStayed();
        void OnTouchCanceled();

        void OnTouchEndedGlobal();
        void OnTouchMovedGlobal();
        void OnTouchStayedGlobal();
        void OnTouchCanceledGlobal();
    }
}