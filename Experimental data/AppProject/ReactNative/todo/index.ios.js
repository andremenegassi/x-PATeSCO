import React, { Component } from 'react';
import {
  AppRegistry,
  StyleSheet,
  Text,
  View,
  TabBarIOS
} from 'react-native';
import Add from './components/add';
import List from './components/list';

export default class Todo extends Component {
  constructor() {
    super();
    this.state = {
      selectedTab: 'todo',
      todos: []
    };
    this.handleListPress = this.handleListPress.bind(this);
    this.handleAddPress = this.handleAddPress.bind(this);
    this.handleAddTodo = this.handleAddTodo.bind(this);
    this.handleDeleteTodo = this.handleDeleteTodo.bind(this);
  }

  handleListPress() {
    this.setState({
      selectedTab: 'todo'
    });
  }

  handleAddPress() {
    this.setState({
      selectedTab: 'add'
    });
  }

  handleAddTodo(todo) {
    let { todos } = this.state;
    todos = todos.slice();
    todos.push(todo);
    this.setState({ todos });
  }

  handleDeleteTodo(todo) {
    let { todos } = this.state;
    todos = todos.slice();
    const index = todos.findIndex(t => t.todo === todo);
    if (index !== -1) {
      todos.splice(index, 1);
      this.setState({ todos });
    }
  }

  render() {
    return (
      <TabBarIOS>
        <TabBarIOS.Item
          title="Todo"
          systemIcon="recents"
          selected={this.state.selectedTab === 'todo'}
          onPress={this.handleListPress}
        >
          <List
            todos={this.state.todos}
            onDeleteTodo={this.handleDeleteTodo}
          />
        </TabBarIOS.Item>
        <TabBarIOS.Item
          title="Add"
          systemIcon="more"
          selected={this.state.selectedTab === 'add'}
          onPress={this.handleAddPress}
        >
          <Add onAddTodo={this.handleAddTodo} />
        </TabBarIOS.Item>
      </TabBarIOS>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#F5FCFF',
  },
  welcome: {
    fontSize: 20,
    textAlign: 'center',
    margin: 10,
  },
  instructions: {
    textAlign: 'center',
    color: '#333333',
    marginBottom: 5,
  },
});

AppRegistry.registerComponent('todo', () => Todo);
