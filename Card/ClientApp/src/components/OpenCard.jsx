import React, { Component } from 'react';
import axios from 'axios';

export class OpenCard extends Component {

    constructor(props) {
        super(props);
        this.state = { name: '', cardType: 'Debit', inn: 12345678 };
    }

    incrementCounter() {
        this.setState({
            currentCount: this.state.currentCount + 1
        });
    }

    updateClientName = name => {
        this.setState({
            name: name.target.value
        });
    }

    updateClientCardType = cardType => {
        this.setState({
            cardType: cardType.target.value
        });
    }

    updateClientInn = inn => {
        this.setState({
            inn: inn.target.value
        });
    }

    callWebApi = param => {
        axios.post(
            'http://localhost:50102/OpenCard/Open',
            JSON.stringify({
                Name: this.state.name,
                Inn: this.state.inn,
                cardType: this.state.cardType
            }),
            {
                headers: { 'Content-Type': 'application/json' },
            }
        )
        .then(response => {
            console.log(response);
        })
        .catch(error => {
            console.log(error);
        });
    }

    render() {
        return (
            <div>
                <h1>Open Card</h1>
                <div>
                    <div><label>Client Name</label><input style={{ marginLeft: "50px" }} type="text" value={this.state.name} onChange={this.updateClientName} /></div>
                    <div><label>Card Type</label><input style={{ marginLeft: "68px" }} type="text" value={this.state.cardType} onChange={this.updateClientCardType} /></div>
                    <div><label>Inn</label><input style={{ marginLeft: "117px" }} type="text" value={this.state.inn} onChange={this.updateClientInn} /></div>
                </div>
                <button className="btn btn-primary" onClick={this.callWebApi}>Open Card</button>
            </div>
        );
    }
}
