namespace LogicoolJapaneseLed.LedControl
{
    internal class LedControllerEcho : LedController
    {
        /// <summary>
        /// Called every frame
        /// </summary>
        public override void Update()
        {
            // Call base update to check language
            base.Update();

            // Additional logic for Echo controller can be added here
            // For now, it just updates the language state
        }
    }
}
