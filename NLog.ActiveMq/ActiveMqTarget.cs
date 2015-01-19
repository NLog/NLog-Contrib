using System;
using System.ComponentModel;
using NLog.Config;
using NLog.Layouts;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;

namespace NLog.Targets
{
	/// <summary>
	/// Writes log message to the specified message queue or topic handled by Apache ActiveMQ.
	/// See http://activemq.apache.org/ for info on Apache ActiveMQ
	/// </summary>
	/// <seealso href="http://nlog-project.org/wiki/ActiveMQ_target">Documentation on NLog Wiki</seealso>
	/// <example>
	/// <p>
	/// To set up the target in the <a href="config.html">configuration file</a>, 
	/// use the following syntax:
	/// </p>
	/// <code lang="XML" source="examples/targets/Configuration File/ActiveMQ/Simple/NLog.config" />
	/// <p>
	/// You can use a single target to write to multiple queues or topics (similar to writing to multiple files with the File target).
	/// </p>
	/// <code lang="XML" source="examples/targets/Configuration File/ActiveMQ/Multiple/NLog.config" />
	/// <p>
	/// The above examples assume just one target and a single rule. 
	/// More configuration options are described <a href="config.html">here</a>.
	/// </p>
	/// <p>
	/// To set up the log target programmatically use code like this:
	/// </p>
	/// <code lang="C#" source="examples/targets/Configuration API/ActiveMQ/Simple/Example.cs" />
	/// </example>
	[Target("ActiveMQ")]
	public class ActiveMqTarget : TargetWithLayout
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ActiveMqTarget"/> class.
		/// </summary>
		/// <remarks>
		/// The default value of the layout is: <code>${longdate}|${level:uppercase=true}|${logger}|${message}</code>
		/// </remarks>
		public ActiveMqTarget()
		{
			Uri = "tcp://localhost:61616";
			Destination = "queue://nlog.messages";
			Persistent = true;
		}

		/// <summary>
		/// Gets or sets the URI to connect to.
		/// </summary>
		/// <remarks>
		/// To connect to the default port on the local host use <c>tcp://localhost:61616</c>
		/// </remarks>
		/// <docgen category='ActiveMQ Options' order='10' />
		[RequiredParameter]
		public string Uri { get; set; }

		/// <summary>
		/// Gets or sets the name of the queue or topic to write to.
		/// </summary>
		/// <remarks>
		/// To write to a queue use <c>queue://queuename</c>.
		/// To write to a topic use <c>topic://queuename</c>.
		/// </remarks>
		/// <docgen category='ActiveMQ Options' order='10' />
		[RequiredParameter]
		public Layout Destination { get; set; }

		/// <summary>
		/// Gets or sets whether persistence is required.
		/// </summary>
		/// <remarks>
		/// Default is true
		/// </remarks>
		/// <docgen category='ActiveMQ Options' order='10' />
		[DefaultValue(true)]
		public bool Persistent { get; set; }

		/// <summary>
		/// Gets or sets the ActiveMQ username.
		/// </summary>
		/// <remarks>
		/// Default is none
		/// </remarks>
		/// <docgen category='ActiveMQ Options' order='10' />
		public string Username { get; set; }

		/// <summary>
		/// Gets or sets the ActiveMQ password.
		/// </summary>
		/// <remarks>
		/// Default is none
		/// </remarks>
		/// <docgen category='ActiveMQ Options' order='10' />
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the ActiveMQ client id.
		/// </summary>
		/// <remarks>
		/// Default is none, which results in a random user id
		/// </remarks>
		/// <docgen category='ActiveMQ Options' order='10' />
		public string ClientId { get; set; }

		/// <summary>
		/// Writes the specified logging event to a queue or topic specified in the Destination 
		/// parameter.
		/// </summary>
		/// <param name="logEvent">The logging event.</param>
		protected override void Write(LogEventInfo logEvent)
		{
			var connecturi = new Uri(Uri);
			var factory = new ConnectionFactory(connecturi);
			if (!String.IsNullOrEmpty(Username))
			{
				factory.UserName = Username;
				factory.Password = Password;
			}
			if (!String.IsNullOrEmpty(ClientId))
				factory.ClientId = ClientId;

			using (var connection = factory.CreateConnection())
			using (var session = connection.CreateSession())
			{
				var destination = SessionUtil.GetDestination(session, Destination.Render(logEvent));
				using (var producer = session.CreateProducer(destination))
				{
					connection.Start();
					producer.DeliveryMode = Persistent
												? MsgDeliveryMode.Persistent
												: MsgDeliveryMode.NonPersistent;

					var logMessage = Layout.Render(logEvent);
					var request = session.CreateTextMessage(logMessage);
					producer.Send(request);
				}
			}
		}
	}
}
