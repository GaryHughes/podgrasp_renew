import React, { Component } from 'react';

export class Library extends Component {
  
    constructor(props) {
    
        super(props);
        
        this.state = {
          podcasts: null,
          isLoading: false,
        };
    
        this.fetchPodcasts = this.fetchPodcasts.bind(this);
        this.setPodcasts = this.setPodcasts.bind(this);
    }

    componentDidMount() {
        this.fetchPodcasts();
    }

    fetchPodcasts() {
        console.log("fetching podcasts");
        this.setState({ isLoading: true });
        fetch('Api/1.0/Podcasts')
            .then(result => this.setPodcasts(result))
            .catch(error => this.setState({ error }));
    }
    
    setPodcasts(result) {
        this.setState({
            podcats: result,
            isLoading: false
        });
    }

    render() {
        return (
        <div>
            Library
        </div>
        );
    }

}
