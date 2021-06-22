import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
            <h1> Simple App for Camunda </h1>
            <p> Process for opening new credit card </p>
            <br/>
            <p> Yo can choose from: </p>
            <ol>
                <li>Open card</li>
                <li>Stop opening process</li>
            </ol>
      </div>
    );
  }
}
