
# 更新日志

## 2018-04-05

### 自动重连的策略更新

之前的逻辑如下：

1. 如果断线，会重连之前的 websocket 地址（prefered server）
2. 并且会一直在 session token 有效期内使用当前的服务器地址

现在的逻辑如下：

1. 如果断线，会重连之前的 websocket 地址（prefered server）
2. 如果重连三次 prefered address 失败，则会切换到 secondary server，并且会重新登录（签名和 session token 都会重新获取并且刷新）
3. 如果 secondary server 也重连失败 3 次，则会重新请求 rtm router 地址，重新获取 prefered server 和 secondary server
4. 重复第一步


## 2017-12-26

### 支持了消息的撤回和修改

消息撤回：

```cs
await this.client.RecallAsync(receivedMessage);// receivedMessage 只要实现了 IAVIMMessage 接口就可以
```

消息修改：
```cs
await this.client.ModifyAysnc(receivedMessage);// receivedMessage 只要实现了 IAVIMMessage 接口就可以
```

而在对话内的其他用户，都会触发对应的事件通知：

```cs
client.OnMessageRecalled += Client_OnMessageRecalled;
private void Client_OnMessageRecalled(object sender, AVIMMessagePatchEventArgs e)
{
    var list = e.Messages.ToList();
    Console.WriteLine(list[0].Id + " has been recalled.");
}          
```

```cs
client.OnMessageModified += Client_OnMessageModified;
private vood Client_OnMessageModified(object sender, AVIMMessagePatchEventArgs e))
{
    var list = e.Messages.ToList();
    Console.WriteLine(list[0].Id + " has been modified.");
}
```

## 2017-12-13
### 支持了已读回执和发送回执的分离

```cs
var realtimeConfig = new AVRealtime.Configuration()
{
    ApplicationId = appId,
    ApplicationKey = appkey,
    OfflineMessageStrategy = AVRealtime.OfflineMessageStrategy.UnreadAck,//开始支持主动发送已读回执
};

realtime = new AVRealtime(realtimeConfig);
```

#### 增加了 AVIMConversation.ReadAsync 和 AVIMClient.ReadAllAsync 接口

```cs
// 告知服务端，当前用户标识自己已经读取了该对话的最新消息，更新已读回执
conversation.ReadAsync();
```

```cs
// 告知服务端，当前用户标识自己所有的对话都已经读取到最新，更新所有的已读回执
client.ReadAllAsync();
```

#### 增加 AVIMConversation.Unread 可以获取当前对话的未读消息状态

```cs
var state = conversation.Unread;
Console.WriteLine(state.UnreadCount);//未读消息数量
Console.WriteLine(state.LastUnreadMessage.Id);//最新一条未读消息的 ID
Console.WriteLine(state.SyncdAt);//最后同步的时间戳
```