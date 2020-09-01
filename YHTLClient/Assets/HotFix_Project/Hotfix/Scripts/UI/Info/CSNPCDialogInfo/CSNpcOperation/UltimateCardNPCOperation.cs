public class UltimateCardNPCOperation : SpecialNpcOperationBase
{
    public override bool DoSpecial(CSAvatar avatar)
    {
        Net.CSRequestCardMessage();
        return true;
    }
}