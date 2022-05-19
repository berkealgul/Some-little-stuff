class Trader {
	constructor(id, cash, crypto) {
		this.id = id;
		this.cash = cash;
		this.crypto = crypto
		
		// sub tree nodes
		this.left = null;
		this.right = null;
	}

	portfolio() {
		return "Cash: " + this.cash + " | Crypto: " + this.crypto;
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

function addTraderToTree(node, id) {
	if(node == null)
		return new Trader(id, 100, 0);
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
		msg.channel.send("Welcome " + msg.author + " and happy trading!");
		root = addTraderToTree(root, msg.author.id);
	} else { // otherwise we send message about unsuccesful operation
		msg.channel.send("Seems you" + msg.author + " are already trading!");
	}
}

function showTraderPortfolio(msg) {
	var trader = getTraderById(msg.author.id);
	if(trader != null)
		msg.channel.send("You currently have: " + trader.portfolio());
}

// for debugging
function printAllTraders(node, num) {
	if(node != null) {
		console.log(num + " " + node.portfolio() + " " + node.id);
		printAllTraders(node.right, num+1);
		printAllTraders(node.left, num+1);
	}
}

function buyCrpyto(msg, args) {
	var trader = getTraderById(msg.author.id);

	if(trader != null) {
		var amount = args[1];
		var cost = amount * price;

		if(cost <= trader.cash) {
			cryptos = cryptos - amount;
			price = crpytoPrice();
			trader.crypto = trader.crypto + amount;
			trader.cash = trader.cash - cost;
			updatePriceData();
			msg.channel.send("You just bought some coin");

		} else {
			msg.channel.send("You dont have enough cash");
		}
	}

}

function sellCrpyto(msg, args) {
	var trader = getTraderById(msg.author.id);

	if(trader != null) {
		var amount = args[1];
		var income = amount * price;

		if(amount <= trader.crypto) {
			cryptos = cryptos + amount;
			price = crpytoPrice();
			trader.crypto = trader.crypto - amount;
			trader.cash = trader.cash + income;
			updatePriceData();
			msg.channel.send("You just sold some coin");
		} else {
			msg.channel.send("You dont have enough crypto");
		}
	}
}

function updatePriceData() {
	var dt = Math.floor((Date.now() - lastHourlyTime) / 1000);

	console.log(hourlyPriceData);
	hourlyPriceData.push(price*10000);

	if(dt >= 3600) {
		hourlyPriceData.push(price*10000);
		lastHourlyTime = Date.now();
	} 

	if(dt >= 3600*24) {
		dailyPriceData.push(price*10000);
		lastDailiyTime = Date.now();
	} 
}

function crpytoPrice() {
	return (total_cryptos-cryptos) / total_cryptos + 0.001;
}

function showCrpytoPrice(msg) {
	msg.channel.send("Current coin price : " + price);
}

// Require the necessary discord.js classes and token
const { Client, Intents, Message } = require('discord.js');
const { token, traderChannel, adminChannel } = require('./config.json');

// Create a new client instance
const client = new Client({
	intents: [
	   Intents.FLAGS.GUILD_MESSAGES,
	   Intents.FLAGS.GUILDS
	]
 });

// global variables
const total_cryptos = 1000;
var cryptos = 1000;
var price = crpytoPrice();
var root = null;
var dailyPriceData = [price*10000];
var hourlyPriceData = [price*10000];
var lastDailiyTime = Date.now();
var lastHourlyTime = Date.now();

// events
client.on('ready', () => { console.log('Ready!'); });
client.on('messageCreate', gotMessage);


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
			buyCrpyto(msg, args);
			break;
		case "sell":
			sellCrpyto(msg, args);
			break;
		case "coin":
			showCrpytoPrice(msg);
			break;
	}
}

function processAdminCommands(msg) {
	var args = msg.content.split(" ");

	switch (args[0]) {
		case "join":
			addTrader(msg);
			break;
		case "debug":
			console.log("Traders");
			printAllTraders(root, 0);
			break;
		case "buy":
			buyCrpyto(msg, args);
			break;
		case "sell":
			sellCrpyto(msg, args);
			break;
		case "coin":
			showCrpytoPrice(msg);
			createChart(msg);
			break;
	}	
}

function createChart(msg) {
	const ChartJsImage = require('chartjs-to-image');

	const data = {
		datasets: [{
		  label: 'Price of the coin',
		  borderColor: 'rgb(255, 99, 132)',
		  data: hourlyPriceData,
		}]
	  };

	const myChart = new ChartJsImage();
	myChart.setConfig({
		type: 'line',
		data: data,
	});
	myChart.toFile('./chart.jpg');

	const chartEmbed = {
		title: 'Shitcoin',
		description: 'Price history of the coin',
		image: {
		  url: myChart.getUrl(),
		},
	};

	msg.channel.send({ embeds: [chartEmbed] });
}

// event handlers
function gotMessage(msg) {
	if(msg.channel.id == traderChannel)
		processTraderCommands(msg);
	else if (msg.channel.id == adminChannel)
		processAdminCommands(msg);
}

// start the bot
client.login(token);
