using System.Collections.Generic;

public static class ListUtils {
	/// Returns the last item in a list
	public static T Last<T>(IList<T> list) {
		if (list.Count == 0) return default(T);
		return list[list.Count - 1];
	}

	/// Returns the last item in a stack
	public static T Last<T>(Stack<T> stack) {
		if (stack.Count == 0) return default(T);
		return stack.Peek();
	}
}
