import React, { Component } from 'react';
import {
  AppRegistry,
  StyleSheet,
  Text,
  View,
  ViewPagerAndroid,
  ToastAndroid
} from 'react-native';
import ToolbarAndroid from 'ToolbarAndroid';
import List from './components/list';
import Add from './components/add';

export default class Todo extends Component {

  constructor() {
    super();
    this.state = {
      todos: []
    };
    this.handleActionSelected = this.handleActionSelected.bind(this);
    this.handleAddTodo = this.handleAddTodo.bind(this);
    this.handleDeleteTodo = this.handleDeleteTodo.bind(this);
  }

  handleActionSelected(position) {
    this.setState({
      actionText: actions[position].title,
    });
    this.pager.setPage(position);
  }

  handleAddTodo(todo) {
    let { todos } = this.state;
    todos = todos.slice();
    todos.push(todo);
    this.setState({ todos });
    ToastAndroid.showWithGravity('Todo is added', ToastAndroid.LONG, ToastAndroid.TOP);
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
      <View style={styles.container}>
        <ToolbarAndroid
          actions={actions}
          onActionSelected={this.handleActionSelected}
          style={styles.toolbar}
          subtitle={this.state.actionText}
        />
        <ViewPagerAndroid
          ref={c => { this.pager = c; }}
          style={styles.viewPager}
          initialPage={0}
        >
          <View>
            <List
              todos={this.state.todos}
              onDeleteTodo={this.handleDeleteTodo}
            />
          </View>
          <View>
            <Add onAddTodo={this.handleAddTodo} />
          </View>
        </ViewPagerAndroid>
      </View>
    );
  }
}

const actions = [
  {
    title: 'Todo',
    show: 'always'
  },
  {
    title: 'Add',
    show: 'always'
  },
];

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'flex-start',
    backgroundColor: '#F5FCFF',
  },
  toolbar: {
    backgroundColor: '#e9eaed',
    height: 56,
  },
  viewPager: {
    flex: 1,
  },
});

AppRegistry.registerComponent('todo', () => Todo);
