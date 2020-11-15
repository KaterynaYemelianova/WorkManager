namespace Exceptions.BusinessLogic
{
    public class InvalidKeyException : ServerException
    {
        public override int Code => 5;
        public override string Message => "Presented key was destroyed or never existed";
    }
}
