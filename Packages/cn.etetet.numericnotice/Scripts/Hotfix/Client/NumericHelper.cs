namespace ET.Client
{
    public static class NumericHelper
    {
        public static async ETTask UpdateNumeric(Scene rootScene)
        {
            C2M_TestNumericValue c2MTestNumericValue = C2M_TestNumericValue.Create();

            M2C_TestNumericValue m2C_TestNumericValue = (M2C_TestNumericValue)await rootScene.GetComponent<ClientSenderComponent>()
                    .Call(c2MTestNumericValue);
        }
    }
}