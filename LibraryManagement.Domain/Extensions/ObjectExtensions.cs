namespace LibraryManagement.Domain.Extensions;

public static class ObjectExtensions
{
    public static T AssertIsNotNull<T>(this T subject) where T : class
    {
        if (ReferenceEquals(null, subject))
            throw new ArgumentNullException(typeof(T).Name);
        return subject;
    }
    
}