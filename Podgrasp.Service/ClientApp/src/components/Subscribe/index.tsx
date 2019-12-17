import React, { Component, FormEvent } from 'react';
import authService from '../api-authorization/AuthorizeService';
import { Subscription, subscribeToPodcast } from '../api-podgrasp';

interface SubscribeProps {};

interface SubscribeState {
  url: string 
};

export class Subscribe extends Component<SubscribeProps, SubscribeState> {
  
  constructor(props: SubscribeProps) {
    super(props);
    this.state = {
      url: ""
    }
    this.onSubmit = this.onSubmit.bind(this);
  }

  componentDidMount() {
    // if (this.input) {
    //   this.input.focus();
    // }

  }

  async onSubmit(event: FormEvent<HTMLFormElement>) {
    try {
      event.preventDefault();
      subscribeToPodcast(new Subscription(this.state.url));
    }
    catch (error) {
      console.error(error);  
    }
  }

  render () {
    return (
      <div>
        <form onSubmit={this.onSubmit}>
          <input 
            type="text"
            onChange={
              (e: React.FormEvent<HTMLInputElement>) => {
                this.setState({ url: e.currentTarget.value });
                console.log(this.state.url);
              }} 
          />
          <span/>
          <button className="btn btn-primary" type="submit">
            Subscribe
          </button>
        </form>
      </div>
    );
  }
}
