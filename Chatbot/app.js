/**** External libraries ****/
const path = require('path');
const express = require('express');
const bodyParser = require('body-parser');
const morgan = require('morgan');
const mongoose = require('mongoose');

/**** Configuration ****/
const appName = "Bot";
const port = (process.env.PORT || 8080);
const app = express();
app.use(bodyParser.json()); // Parse JSON from the request body
app.use(morgan('combined')); // Log all requests to the console
app.use(express.static(path.join(__dirname, '../build')));

mongoose.connect("mongodb+srv://dbadmin:UXGsPBy3YpKcXrjn@jobsite-btkxb.azure.mongodb.net/test?retryWrites=true&w=majority", {useNewUrlParser: true});
const db = mongoose.connection;
db.on('error', console.error.bind(console, 'console error:'));
db.once('open', function () {
    console.log('We are connected!');
});
// Additional headers for the response to avoid trigger CORS security
// errors in the browser
// Read more here: https://en.wikipedia.org/wiki/Cross-origin_resource_sharing
app.use((req, res, next) => {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Authorization, Origin, X-Requested-With, Content-Type, Accept");
    res.header("Access-Control-Allow-Methods", "GET, POST, OPTIONS, PUT, DELETE");

    // intercepts OPTIONS method
    if ('OPTIONS' === req.method) {
      // respond with 200
      console.log("Allowing OPTIONS");
      res.send(200);
    }
    else {
      // move on
      next();
    }
});

let screwdriverSchema = new mongoose.Schema(
);

let hammerSchema = new mongoose.Schema(
    {
        url:String
    }
);

const Hammer = mongoose.model('hammer', hammerSchema);
const ScrewDriver = mongoose.model('screwdriver', screwdriverSchema);


/**** Routes ****/
app.get('/api/job', (req, res) =>
{
    Job.find((er,docs) => res.send(docs))
        .populate([
            {path: 'category', model: 'Categories'},
            {path: 'area', model: 'Areas'},
        ])
});

app.get('/api/job/:id', (req, res) =>
{
    Job.findOne({
        _id: req.params.id}, (err, job) =>{
        if(err) {
            console.log(err);
        }
        else
        {
            res.json(job);
            res.send();
            console.log(job, "app");
        }
    })
});

app.get('/api/category', (req, res) =>
{
    Category.find((er, docs)=>{
        res.send(docs);
    });
});

app.get('/api/area', (req, res) =>
{
    Area.find((er, docs)=>{
        res.send(docs);
    });
});


/**** Reroute all unknown requests to the React index.html ****/
app.get('/*', (req, res) => {
  res.sendFile(path.join(__dirname, '../build/index.html'));
});

/**** Start! ****/
app.listen(port, () => console.log(`${appName} API running on port ${port}!`));




