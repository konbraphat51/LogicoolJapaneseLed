namespace LogicoolJapaneseLed
{
    internal abstract class LedController
    {
        protected bool isJapanese = false;
        protected bool hasContext = false;

        /// <summary>
        /// Called every frame
        /// </summary>
        public virtual void Update()
        {
            (isJapanese, hasContext) = LanguageDetection.IsJapanese();
        }
    }
}
