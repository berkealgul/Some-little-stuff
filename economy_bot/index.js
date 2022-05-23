// imports
const { Client, Intents, Message, MessageContextMenuInteraction } = require('discord.js');
const fs = require('fs');


class Trader {
	constructor(id, cash, coin) {
		this.id = id;
		this.cash = cash;
		this.coin = coin
		
		// sub tree nodes
		this.left = null;
		this.right = null;
	}

	portfolio() {
		return "wallet: " + this.cash + " | coin: " + this.coin;
	}
};

function getTraderById(id) {
	let head = root;

	while(head != null) {
		if(head.id > id)
			head = head.left;
		else if(head.id < id)
			head = head.right;
		else
			return head;
	}
	
	return null;
}

function addTraderToTree(node, id, cash=100, coin=0) {
	if(node == null)
		return new Trader(id, cash, coin);
	else if(node.id > id) {
		node.left = addTraderToTree(node.left, id);
		return node;
	} else {
		node.right = addTraderToTree(node.right, id);
		return node;
	}
}

function addTrader(msg) {
	// if trader not exisits we add new one
	if(getTraderById(msg.author.id) == null) {
		addTraderToTree(msg.author.id); 
		msg.reply("Welcome " + msg.author + " and happy trading!");
		root = addTraderToTree(root, msg.author.id);
	} else { // otherwise we send message about unsuccesful operation
		msg.reply("Seems you are already trading!");
	}
}

function showTraderPortfolio(msg) {
	var trader = getTraderById(msg.author.id);
	if(trader != null)
		msg.reply("You currently have: " + trader.portfolio());
}

// for debugging
function printAllTraders(node, num) {
	if(node != null) {
		console.log(num + " " + node.portfolio() + " " + node.id);
		printAllTraders(node.right, num+1);
		printAllTraders(node.left, num+1);
	}
}

function buyCoin(msg, args) {
	var trader = getTraderById(msg.author.id);

	if(trader != null) {
		var amount = args[1];
		var cost = amount * price;

		if(cost <= trader.cash) {
			volume = volume + amount;
			trader.coin = Number(trader.coin) + Number(amount);
			trader.cash = Number(trader.cash) - Number(cost);
			price = crpytoPrice();
			msg.reply("Nice! You just bought" + amount + "coin");

		} else {
			msg.reply("You dont have enough cash");
		}
	}

}

function sellCoin(msg, args) {
	var trader = getTraderById(msg.author.id);

	if(trader != null) {
		var amount = args[1];
		var income = amount * price;

		if(amount <= trader.coin) {
			volume = volume - amount;
			trader.coin = Number(trader.coin) - Number(amount);
			trader.cash = Number(trader.cash) + Number(income);
			msg.reply("Nice! You just" + amount + "coin");
			price = crpytoPrice();
		} else {
			msg.reply("You dont have enough coin");
		}
	}
}

function crpytoPrice() {
	return (totalVolume-volume) / totalVolume + 0.001;
}

function showCrpytoPrice(msg) {
	msg.channel.send("Current coin price : " + price);
}

function showTraderBalance(msg) {
	var trader = getMentionedTrader(msg);
	
	if(trader == null) {
		msg.reply("Trader is not found!");
	} else {
		msg.channel.send(parseMention(trader.id)+" balance: " + trader.portfolio());
	}

}

// returns FIRST mentioned trader in msg null if trader is not joined or there is no mentioned user
function getMentionedTrader(msg) {
	var user = msg.mentions.users.first();

	if(user == undefined) { 
		return null; 
	} else {
		return getTraderById(user.id); 
	}
}

function getDateNowAsUTC() {
	return new Date().toUTCString();
}

function saveTradersToJson() {
	if(root == null) { 
		console.log("Couldnt save because there is no trader to save :(");
		return; 
	}
	
	var stack = [root]; 

	var jsonData = {
		date : getDateNowAsUTC(),
		traderCount : 0,
		traders : [],
	};

	while(stack.length > 0) {
		var trader = stack.pop();

		// todo: create json object from trader and add to the list
		var traderData = {
			"id" : trader.id,
			"cash" : trader.cash,
			"coin" : trader.coin 
		};

		jsonData.traders.push(traderData);

		if(trader.left != null) { stack.push(trader.left); }
		if(trader.right != null) { stack.push(trader.right); }

		jsonData.traderCount++;
	}

	fs.writeFile('./traders.json', JSON.stringify(jsonData, null, 4), function(err) {
		if(err) {
			console.log(err);
		} else {
			console.log("JSON saved to /traders.json");
		}
	}); 
}

function loadTradersFromJson() {
	var rawdata = fs.readFileSync('./traders.json');
	var traderData = JSON.parse(rawdata);

	for(var i = 0; i < traderData.traderCount; i++) {
		var trader = traderData["traders"][i];
		root = addTraderToTree(root, trader["id"], trader["cash"], trader["coin"]);
		volume += Number(trader["coin"]); // increase the colume
	}

	console.log("Traders Loaded");
}

// command give [amount] [coin/cash] [user]
function giveTraderAsset(msg, args) {
	var trader = getMentionedTrader(msg);

	if(trader == null) {
		msg.reply("Trader not found");
		return;
	}

	if(args[2] == 'coin') {
		var amount = Math.min(args[1], totalVolume-volume);
		volume += amount;
		price = crpytoPrice();
		trader.coin += amount;
		msg.channel.send("You gave " + amount + " coin to " + parseMention(trader.id) + ". Current balance: " + trader.portfolio());
	} else if(args[2] == 'cash') {
		trader.cash += Number(args[1]);
		msg.channel.send("You gave " + args[1] + " cash to " + parseMention(trader.id) + ". Current balance: " + trader.portfolio());
	}
}

// command give  [amount] [coin/cash] [user]
function takeTraderAsset(msg, args) {
	var trader = getMentionedTrader(msg);

	if(trader == null) {
		msg.reply("Trader not found");
		return;
	}

	if(args[2] == 'coin') {
		var amount = Math.max(args[1], 0);
		volume -= amount;
		price = crpytoPrice();
		trader.coin -= amount;
		msg.channel.send("You took " + amount + " coin from " + parseMention(trader.id) + ". Current balance: " + trader.portfolio());
	} else if(args[2] == 'cash') {
		var amount = Math.max(args[1], 0);
		trader.cash -= amount;
		msg.channel.send("You took " + amount + " cash from " + parseMention(trader.id) + ". Current balance: " + trader.portfolio());
	}
}

function parseMention(id) {
	return "<@"+id+">";
}

function processTraderCommands(msg) {
	var args = msg.content.split(" ");

	switch (args[0]) {
		case "join":
			addTrader(msg);
			break;
		case "portfolio":
			showTraderPortfolio(msg);
			break;
		case "buy":
			buyCoin(msg, args);
			break;
		case "sell":
			sellCoin(msg, args);
			break;
		case "price":
			createChart(msg);
			break;
	}
}

function processAdminCommands(msg) {
	var args = msg.content.split(" ");

	switch (args[0]) {
		case "balance":
			showTraderBalance(msg);
			break;
		case "give":
			giveTraderAsset(msg, args);
			break;
		case "take":
			takeTraderAsset(msg, args);
			break;
		case "remove":
			removeTraderAsset(msg, args);
			break;
		case "save":
			saveTradersToJson();
			msg.reply({
				files: [{
					attachment: './traders.json',
					name: 'traders.json'
				}],
				content: "Traders are saved successfully",
			});
			break;
		case "debug":
			console.log("Traders");
			printAllTraders(root, 0);
			break;
	}	
}

// event handlers
function gotMessage(msg) {
	if(msg.author.id == botId) { return; } // dont process the messages from bot

	if(msg.channel.id == traderChannel)
		processTraderCommands(msg);
	else if (msg.channel.id == adminChannel)
		processAdminCommands(msg);
}


// below is main zone

// get config data from config.json
const { token, traderChannel, adminChannel, botId } = require('./config.json');

// Create a new client instance
const client = new Client({
	intents: [
	   Intents.FLAGS.GUILD_MESSAGES,
	   Intents.FLAGS.GUILDS
	]
});

// global variables
const totalVolume = 1000;
var volume = 0;
var price = crpytoPrice();
var root = null;

// events
client.on('ready', () => { 
	loadTradersFromJson();
	console.log('Ready!'); 
});
client.on('messageCreate', gotMessage);

// start the bot
client.login(token);
