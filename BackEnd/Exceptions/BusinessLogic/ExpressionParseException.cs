namespace Exceptions.BusinessLogic
{
    public class ExpressionParseException : ServerException
    {
        public override int Code => 19;
        public override string Message => "Failed to parse expression";
    }
}
