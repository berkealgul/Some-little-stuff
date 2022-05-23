// imports
const { Client, Intents, Message, MessageContextMenuInteraction, MessageEmbed } = require('discord.js');
const fs = require('fs');


class Trader {
	constructor(id, cash, coin) {
		this.id = id;
		this.cash = cash;
		this.coin = coin;
		
		// sub tree nodes
		this.left = null;
		this.right = null;
	}

	portfolio() {
		const portfolioEmbed = {
			color: 0x0099ff,
			author: {
				name: 'Crypto Bot',
			},
	
			description: "Portfolio of" + parseMention(this.id),
			
			fields: [
				{
					name: 'Wallet',
					value: this.cash.toString() + "$",
					inline: true
				},
				{
					name: 'Coin',
					value: this.coin.toString(),
					inline: true
				},
			],
		};
	
		return portfolioEmbed;
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

function addTraderToTree(node, id, cash=traderStartingCash, coin=0) {
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
		msg.channel.send("Welcome " + parseMention(msg.author.id) + " and happy trading!");
		root = addTraderToTree(root, msg.author.id);
	} else { // otherwise we send message about unsuccesful operation
		msg.reply("Seems you are already trading!");
	}
}

function showTraderPortfolio(msg, dm) {
	var trader = getTraderById(msg.author.id);
	if(trader != null) {
		if(dm != undefined) {
			msg.author.send({ embeds: [trader.portfolio()] });
		}
		else {
			msg.channel.send({ embeds: [trader.portfolio()] });
		}
	}	
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
		} else {
			msg.reply("You dont have enough coin");
		}
	}
}

function showCoinPrice(msg) {
	const coinEmbed = {
		color: 0x0099ff,
		author: {
			name: 'Crypto Bot',
		},

		description: 'Price change in: '  + getTimeToPriceChange() + " min" +"\n------------------------------",
		
		fields: [
			{
				name: 'Price',
				value: getPriceIndicationString()
			},
			{
				name: 'Volume',
				value: volume.toString()
			}
		]
	};
	
	msg.channel.send({ embeds: [coinEmbed] });
}

function getPriceIndicationString() {
	var dt = 100*(price - oldPrice) / oldPrice;
	var plus = '';

	if (dt > 0) { 
		plus = '+';
	}

	return price + "$ (" + plus + 100*(price - oldPrice) / oldPrice + "%)";
}

function getTimeToPriceChange() {
	var seconds = (Date.now()-lastPriceChange) / 1000;
	return priceRefleshRate - Math.floor(seconds / 60);
}

function calculateCoinPrice() {
	return volume * pricePerVolume + minPrice;
}

function updateCoinPrice() {
	if(getTimeToPriceChange() > 0) {
		return;
	}

	lastPriceChange = new Date(); // change time
	oldPrice = price;
	price = calculateCoinPrice();
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
		ppv : pricePerVolume,
		mp : minPrice,
		traderCount : 0,
		traders : [],
	};

	while(stack.length > 0) {
		var trader = stack.pop();

		// todo: create json object from trader and add to the list
		var botData = {
			"id" : trader.id,
			"cash" : trader.cash,
			"coin" : trader.coin 
		};

		jsonData.traders.push(botData);

		if(trader.left != null) { stack.push(trader.left); }
		if(trader.right != null) { stack.push(trader.right); }

		jsonData.traderCount++;
	}

	fs.writeFile('./bot-params.json', JSON.stringify(jsonData, null, 4), function(err) {
		if(err) {
			console.log(err);
		} else {
			console.log("JSON saved to /bot-params.json");
		}
	}); 
}

function loadFromJson() {
	var rawdata = fs.readFileSync('./bot-params.json');
	var botData = JSON.parse(rawdata);

	for(var i = 0; i < botData.traderCount; i++) {
		var trader = botData["traders"][i];
		root = addTraderToTree(root, trader["id"], trader["cash"], trader["coin"]);
		volume += Number(trader["coin"]); // increase the colume
	}

	minPrice = botData["mp"];
	pricePerVolume = botData["ppv"];

	console.log("Params Loaded");
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
		trader.coin += amount;
		msg.channel.send("You gave " + amount + " coin to " + parseMention(trader.id) + ". Current balance: " + trader.portfolio());
	} else if(args[2] == 'cash') {
		trader.cash += Number(args[1]);
		msg.channel.send("You gave " + args[1] + " cash to " + parseMention(trader.id) + ". Current balance: " + trader.portfolio());
	}
}

// command give [amount] [coin/cash] [user]
function takeTraderAsset(msg, args) {
	var trader = getMentionedTrader(msg);

	if(trader == null) {
		msg.reply("Trader not found");
		return;
	}

	if(args[2] == 'coin') {
		var amount = Number(args[1]);
		if(trader.coin-amount < 0) {
			amount = trader.coin;
		}
		volume -= amount;
		trader.coin -= amount;
		msg.channel.send("You took " + amount + " coin from " + parseMention(trader.id) + ". Current balance: " + trader.portfolio());

	} else if(args[2] == 'cash') {
		var amount = Number(args[1]);
		if(trader.cash-amount < 0) {
			amount = trader.cash;
		}
		trader.cash -= amount;
		msg.channel.send("You took " + amount + " cash from " + parseMention(trader.id) + ". Current balance: " + trader.portfolio());
	}
}

function save(msg) {
	saveTradersToJson();
	msg.reply({ // attach json to msg so that it can be downloaded
		files: [{
			attachment: './bot-params.json',
			name: 'bot-params.json'
		}],
		content: "Traders are saved successfully",
	});
}

function parseMention(id) {
	return "<@"+id+">";
}

// format change [parameter to change] [new value]
function changeCoinPrice(msg, args) {
	if(args[2] == undefined) {
		msg.reply("Value not specified");
		return;
	}

	if(args[1] === "ppv") {
		pricePerVolume = Number(args[2]);
	} else if(args[1] === "mp") {
		minPrice = Number(args[2]);
	} else {
		return;
	}

	// send embed on success
	const infoEmbed = {
		color: 0x0099ff,
		author: {
			name: 'Crypto Bot',
		},

		description:  "Change success! (Prices will be updated with new variables)",
		
		fields: [
			{
				name: 'Min Price (mp)',
				value: minPrice.toString(),
				inline: true
			},
			{
				name: 'Price Per Volume (ppv)',
				value: pricePerVolume.toString(),
				inline: true
			},
			{
				name: '------------',
				value: 'Price change in: '  + getTimeToPriceChange() + " min",
				inline: false
			},
		],
	};

	msg.channel.send({ embeds: [infoEmbed] });
}

function showPriceParams(msg) {
	msg.channel.send("Min Price: " + minPrice + "| Price Per Volume: " + pricePerVolume);
}

function showAdminHelp(msg) {
		const helpEmbed = {
			color: 0x0099ff,

			author: {
				name: 'Crypto Bot',
			},
			
			description: "Available commands for admins",

			fields: [
				{
					name: 'balance [@user]',
					value: "Displays portfolio of user",
				},
				{
					name: 'give/take [amount] [coin/cash] [@user]',
					value: 'Gives or takes asset(coin or cash) to user with specified amount',
				},
				{
					name: 'save',
					value: "Saves current bot parameters.",
				},
				{
					name: 'change [mp/ppv] [value]',
					value: "Changes price parameters (mp->min price ppv->price per volume). to new value",
				},
				{
					name: 'priceParams',
					value: "Shows current price params",
				},
				{
					name: 'price',
					value: "Shows current price information of the coin",
				},
				{
					name: 'help',
					value: "to send this message again if you forget :)",
				},
			],
		};
	
		msg.channel.send({ embeds: [helpEmbed] });
}

function showTraderHelp(msg) {
	const helpEmbed = {
		color: 0x0099ff,

		author: {
			name: 'Crypto Bot',
		},
		
		description: "Available commands for traders",

		fields: [
			{
				name: 'join',
				value: "Signs you up to trading system.",
			},
			{
				name: 'price',
				value: "Shows current price information of the coin",
			},
			{
				name: 'portfolio [optional anything]',
				value: "Shows your current assets if you write anything to second parameter the bot will dm you",
			},
			{
				name: 'buy/sell [amount]',
				value: "buy or sell specified amount of coin",
			},
			{
				name: 'help',
				value: "to send this message again if you forget :)",
			},
		],
	};

	msg.channel.send({ embeds: [helpEmbed] });
}

function processTraderCommands(msg) {
	var args = msg.content.split(" ");

	switch (args[0]) {
		case "join":
			addTrader(msg);
			break;
		case "portfolio":
			showTraderPortfolio(msg, args[1]);
			break;
		case "buy":
			buyCoin(msg, args);
			break;
		case "sell":
			sellCoin(msg, args);
			break;
		case "price":
			showCoinPrice(msg);
			break;
		case "help":
			showTraderHelp(msg);
			break;
		default:
			msg.reply("Unvalid command");
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
		case "save":
			save(msg);
			break;
		case "priceParams":
			showPriceParams(msg);
			break;
		case "price":
			showCoinPrice(msg);
			break;
		case "change":
			changeCoinPrice(msg, args);
			break;
		case "help":
			showAdminHelp(msg);
			break;
		case "debug": // not useful for end user
			console.log("Traders");
			printAllTraders(root, 0);
			break;
		default:
			msg.reply("Unvalid command");
	}	
}

// event handlers
function onMessage(msg) {
	if(msg.author.id == botId) { return; } // dont process the messages from bot

	updateCoinPrice();

	if(msg.channel.id == traderChannel)
		processTraderCommands(msg);
	else if (msg.channel.id == adminChannel)
		processAdminCommands(msg);
}

function onReady() {
	loadFromJson();
	console.log('Ready!'); 
}
// below is main zone

// get config data from config.json
const { token, traderChannel, adminChannel, botId,
	 totalVolume, priceRefleshRate, traderStartingCash} = require('./config.json');

// global variables
var volume = 0; // volume will change with data from json
var minPrice = 5; // price controlling variables;
var pricePerVolume = 5; // also they will come from json
var price = calculateCoinPrice();
var oldPrice = price;
var root = null;
var lastPriceChange = new Date();

// Create a new client instance
const client = new Client({
	intents: [
	   Intents.FLAGS.GUILD_MESSAGES,
	   Intents.FLAGS.GUILDS
	]
});

// events
client.on('ready', onReady);
client.on('messageCreate', onMessage);

// start the bot
client.login(token);
