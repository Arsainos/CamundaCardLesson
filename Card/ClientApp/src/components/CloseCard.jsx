import React, { Component } from 'react';
import axios from 'axios';

export class CloseCard extends Component {

    constructor(props) {
        super(props);       
    }

    callWebApi = param => {
        axios.post(
            'http://localhost:50102/OpenCard/Stop'
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
                <p> Are you sure to stop opening card process </p>

                <button className="btn btn-primary" onClick={this.callWebApi}>Close Card</button>
            </div>
        );
    }
}
