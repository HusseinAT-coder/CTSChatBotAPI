# WhatsApp-Style ChatBot API

This project is a mock chat engine built with .NET 8, mimicking WhatsApp-style messaging and OpenAI replies.

Features

- Send/receive user messages
- Mocked OpenAI AI replies with multi-turn context
- Retry logic on AI failures
- Admin endpoint for chat history
- In-memory database
- Swagger UI for testing

Technologies
- .NET 8 Web API
- Entity Framework Core (In-Memory)
- Swagger

Endpoints

'POST /api/messages/receive'
Receive a user message
json
{
  "userId": "001",
  "content": "hello"
}

'POST /api/messages/send'
reply an ai message
json
{
  "userId": "001"
}

'GET /api/admin/conversations'
Get grouped conversation history per user

Swagger available at: https://localhost:{port}/swagger

#Clone repo
git clone https://github.com/HusseinAT-coder/CTSChatBotAPI.git