namespace Command
{
    public class MoveCommand : Command
    {
        private PlayerMovement _playerMovement;
        private float _h, _v;

        public MoveCommand(PlayerMovement playerMovement, float h, float v)
        {
            _playerMovement = playerMovement;
            _h = h;
            _v = v;
        }
        //Trigger perintah movement
        public override void Execute()
        {
            _playerMovement.Move(_h, _v);
            //Menganimasikan player

            _playerMovement.Animating(_h, _v);
        }

        public override void UnExecute()
        {
            //Invers arah dari movement player
            _playerMovement.Move(-_h, -_v);
            
            //Menganimasikan player
            _playerMovement.Animating(_h, _v);
        }
    }
}
