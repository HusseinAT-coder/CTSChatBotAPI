namespace CTSChatBotAPI.Services
{
    public class MockAIService
    {
        public string Execute(List<string> conversationHistory)
        {
            // feed our prompt with prev conversations here 
            // In this case 
            // fetching latest message recieved 
            var lastUserMessage = conversationHistory.LastOrDefault() ?? "";

            // Simple mock data 
            // In case the content contain "Hello" word
            if (lastUserMessage.Contains("hello", StringComparison.OrdinalIgnoreCase))
                // Return a mock message
                return "Hello, How can I help you today?";

            // In case the content contain "Weather" word
            if (lastUserMessage.Contains("weather", StringComparison.OrdinalIgnoreCase))
                // return a mock message
                return "The weather is sunny and nice.";

            // return a default mock message in other cases
            return "I'm just a mock bot, page my creator for extra info";
        }

        public string GenerateReply(List<string> history, out bool isFailed)
        {
            isFailed = false; // by default ai is successfull 

            int attempt = 0; // initiate attempts

            while (attempt < 3) // random max attempt number --> could be a parameter
            {
                try
                {
                    // Simulate random failure
                    if (Random.Shared.Next(0, 4) == 0) // get random number between 0 and 4
                        throw new Exception("AI error");

                    return Execute(history);
                }   
                catch
                {
                    // in case it failed
                    attempt++; // increment attempt

                    if (attempt == 3) // if max attempts reached 
                    {
                        // return a failure status
                        isFailed = true;
                        return "Sorry, Try again later.";
                    }
                }
            }

            // In case of a fallback
            isFailed = true;
            return "Retry failed";
        }
    }
}
