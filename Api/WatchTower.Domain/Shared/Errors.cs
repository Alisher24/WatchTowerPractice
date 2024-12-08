namespace WatchTower.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "Value";
            
            return Error.Validation("value.is.invalid", 
                $"{label} is invalid");
        }
        public static Error NotFound(string? name = null)
        {
            var label = name ?? "Value";

            return Error.Validation("value.not.found",
                $"{label} not found");
        }
        
        public static Error ValueIsAlreadyExists(string? name = null)
        {
            var label = name ?? "Value";
            
            return Error.Validation("value.is.already.exists", 
                $"{label} already exists");
        }
        
        public static Error ValueIsBeingUsedByAnotherObject(string? name = null)
        {
            var label = name ?? "Value";
            
            return Error.Validation("value.is.being.used.by.another.object", 
                $"{name} is being used by another object");
        }
    }
}