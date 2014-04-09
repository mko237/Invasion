# Copyright (c) Twisted Matrix Laboratories.
# See LICENSE for details.


"""
An example IRC log bot - logs a channel's events to a file.

If someone says the bot's name in the channel followed by a ':',
e.g.

    <foo> logbot: hello!

the bot will reply:

    <logbot> foo: I am a log bot

Run this script with two arguments, the channel name the bot should
connect to, and file to log to, e.g.:

    $ python ircLogBot.py test test.log

will log channel #test to the file 'test.log'.

To run the script:

    $ python ircLogBot.py <channel> <file>
"""


# twisted imports
from twisted.words.protocols import irc
from twisted.internet import reactor, protocol, task
from twisted.python import log


from twisted.internet.protocol import ClientFactory
from twisted.protocols.basic import LineReceiver

# system imports
import time, sys
from time import sleep
from multiprocessing import Process,Queue,Lock


def isCommand(s):
        #print 'checking cmd'
        firstDigit = s[0]
        S = s.upper()
        try:
            int(firstDigit)
            return True
        except:
            if firstDigit == "*":
                return True
            elif S == "GIVE UP" or S == "GIVEUP" :
                    #print "give up true"
                    return True
            else:
                return False
def cleanMsg(s):
        S = s
        if s[0] == 'g' or s[0] == 'G':
                S = s.upper()
        return S
        
        


class MessageLogger:
    """
    An independent logger class (because separation of application
    and protocol logic is a good thing).
    """
    #def __init__(self, file):
        #self.file = file
    
    def __init__(self,file = None,queue = None):
        self.queue = queue
    def log(self, message):
        """Write a message to the file."""
        #self.queue.Enqueue(message)
        #print 'wrote to q'
        
        #timestamp = time.strftime("[%H:%M:%S]", time.localtime(time.time()))
        #self.file.write('%s %s\n' % (timestamp, message))
        #self.file.flush()

    def close(self):
        #self.file.close()
        pass


class LogBot(irc.IRCClient):
    """A logging IRC bot."""
    
    nickname = "InvasionOnTwitch"
    password = "oauth:l7g10vpxshg7beo4mmtptzlra39qnfy"
    
    def connectionMade(self):
        irc.IRCClient.connectionMade(self)
        self.logger = MessageLogger(queue = self.factory.queue)#open(self.factory.filename, "a"))
        #self.logger.log("[connected at %s]" % 
         #               time.asctime(time.localtime(time.time())))

    def connectionLost(self, reason):
        irc.IRCClient.connectionLost(self, reason)
        #self.logger.log("[disconnected at %s]" % 
         #               time.asctime(time.localtime(time.time())))
        self.logger.close()


    # callbacks for events

    def signedOn(self):
        """Called when bot has succesfully signed on to server."""
        self.join(self.factory.channel)

    def joined(self, channel):
        """This will get called when the bot joins the channel."""
        #self.logger.log("[I have joined %s]" % channel)
    
            
        

    def privmsg(self, user, channel, msg):
        """This will get called when the bot receives a message."""
        user = user.split('!', 1)[0]
        #self.logger.log("<%s> %s" % (user, msg))
        #print (msg)
        if isCommand(msg):
            self.factory.queue.Enqueue(user+"<!>"+cleanMsg(msg))
        #print "Enqueued"
        
        
        
        # Check to see if they're sending me a private message
        if channel == self.nickname:
            msg = "It isn't nice to whisper!  Play nice with the group."
            self.msg(user, msg)
            return

        # Otherwise check to see if it is a message directed at me
        if msg.startswith(self.nickname + ":"):
            msg = "%s: I am a log bot" % user
            self.msg(channel, msg)
            self.logger.log("<%s> %s" % (self.nickname, msg))

    def action(self, user, channel, msg):
        """This will get called when the bot sees someone do an action."""
        user = user.split('!', 1)[0]
        #self.logger.log("* %s %s" % (user, msg))

    # irc callbacks

    def irc_NICK(self, prefix, params):
        """Called when an IRC user changes their nickname."""
        old_nick = prefix.split('!')[0]
        new_nick = params[0]
        #self.logger.log("%s is now known as %s" % (old_nick, new_nick))


    # For fun, override the method that determines how a nickname is changed on
    # collisions. The default method appends an underscore.
    def alterCollidedNick(self, nickname):
        """
        Generate an altered version of a nickname that caused a collision in an
        effort to create an unused related name for subsequent registration.
        """
        return nickname + '^'



class LogBotFactory(protocol.ClientFactory):
    """A factory for LogBots.

    A new protocol instance will be created each time we connect to the server.
    """

    def __init__(self, channel, filename,queue):
        self.channel = channel
        self.filename = filename
        self.queue = queue

    def buildProtocol(self, addr):
        p = LogBot()
        p.factory = self
        return p

    def clientConnectionLost(self, connector, reason):
        """If we get disconnected, reconnect to server."""
        connector.connect()

    def clientConnectionFailed(self, connector, reason):
        print "connection failed:", reason
        #reactor.stop()
        connector.connect()

def startIRC(queue):

    # initialize logging
    log.startLogging(sys.stdout)
    
    # create factory protocol and application
    f = LogBotFactory('invasionontwitch', 'd:/downloads/irctest/threadtest.txt', queue)

    # connect factory to this host and port
    reactor.connectTCP("irc.twitch.tv", 6667, f)

    # run bot
    #reactor.run()

###################################################################################
    ###################################################################################
    ############################################################################
    ############################################################################

class EchoClient(LineReceiver):
    end="Bye-bye!"
    x = 0
    def __init__(self,queue):
        self.queue = queue
        self.task_id = task.LoopingCall(self.sendQueue)
    def sendQueue(self):
        if( not self.queue.Empty()):
            self.sendLine(queue.Dequeue())
            #print self.queue.Count()
    def connectionMade(self):
        #self.sendLine("Still goin..." + str(self.x))
        #self.x+=1
        #print 'connected'
        self.sendLine('connected')
        
        self.task_id.start(.008)
       
        
        
      
        #self.sendLine("Hello, world!")
        #self.sendLine("What a fine day it is.")
        #self.sendLine(self.end)
        
         #   self.sendLine("Still goin...")
          #  sleep(10)
            

    def lineReceived(self, line):
        print "receive:", line
        #sleep(.008)
        #self.sendLine("Still goin..." + str(self.x))
        #self.x+=1
        
        if line==self.end:
            self.transport.loseConnection()

class EchoClientFactory(ClientFactory):
    
    protocol = EchoClient


    #def __init__(queue):
     #   protocol.queue = queue
        
    def clientConnectionFailed(self, connector, reason):
        print 'connection failed:', reason.getErrorMessage()
        print 'trying again'
        sleep(1)
        connector.connect()
        
        

    def clientConnectionLost(self, connector, reason):
        print 'connection lost:', reason.getErrorMessage()
        #eactor.stop()
        connector.connect()

    def buildProtocol(self, addr):
        p = self.protocol(self.queue)
        p.factory = self
        return p

def startIS(queue):
    factory = EchoClientFactory()
    factory.queue = queue
    reactor.connectTCP('localhost', 8221, factory)
    #R2.run()

class Q(object):
    def __init__(self, initval=0):
        self.val = Queue()
        self.lock = Lock()
        
        

    def Enqueue(self,msg):
        with self.lock:
            self.val.put(msg)
    def Dequeue(self):
        with self.lock:
            return self.val.get()
    def Empty(self):
        return self.val.empty()
    def Count(self):
        return self.val.qsize()
        
        

if __name__ == '__main__':
    queue = Q()
    #startIS()
   
    p1 = Process(target = startIRC, args = (queue,))
    p2 = Process(target = startIS, args = (queue,))

    p2.run()
    p1.run()
    
    reactor.run()

    
