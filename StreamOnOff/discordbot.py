import os
from dotenv import load_dotenv
import discord
import webbrowser

client = discord.Client()

load_dotenv(dotenv_path=r'C:\Users\mikel\Desktop\Proftaak 2.0\discordbot.env')

DISCORD_TOKEN=os.getenv("DISCORD_TOKEN")
DISCORD_GUILD=os.getenv("DISCORD_GUILD")

# @client.event
# async def on_ready():
    # for guild in client.guilds:
    #     for channel in guild.channels:
    #         if str(channel)=="general":
    #             async for msg in channel.history(limit=100):
    #                 print(msg.content)
    # for guild in client.guilds:
    #     for member in guild.members:
    #         if member.id!= 647352570309902336:
    #             print(member.name)
                # await member.send("test")

@client.event
async def on_message(message):
    global bericht
    bericht = message.content
    if bericht == "aan":
        print("aan")
        # webbrowser.open ('http://localhost/admin?state=aan')
    elif bericht == "uit":
        print("uit")
        # webbrowser.open ('http://localhost/admin?state=uit')
# @client.event
# async def on_member_join(member):
#     print(member)
#     await member.create_dm()
#     await member.dm_channel.send(
#         f'Hi {member.name}, welcome to my Discord server!'
#     )

if __name__ == "__main__":
    print("run1")
    client.run(DISCORD_TOKEN)
