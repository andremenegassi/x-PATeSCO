import React, { Component } from 'react';
import {
  StyleSheet,
  TextInput,
  View,
  TouchableHighlight,
  Text,
  ListView,
  Platform
} from 'react-native';
import Error from './error';

export default class List extends Component {
  constructor(props) {
    super(props);
    this.dataSource = new ListView.DataSource({rowHasChanged: (r1, r2) => r1 !== r2});
    this.state = {
      dataSource: this.dataSource.cloneWithRows(props.todos)
    };
    this.handleDeletePress = this.handleDeletePress.bind(this);
  }

  componentWillReceiveProps(nextProps) {
    if (nextProps.todos !== this.props.todos) {
      this.setState({
        dataSource: this.dataSource.cloneWithRows(nextProps.todos)
      });
    }
  }

  handleDeletePress(text) {
    this.props.onDeleteTodo(text);
  }

  render() {
    return (
      <View style={styles.container}>
        <Text style={styles.title}>Todo</Text>
        {this.props.todos.length === 0 ? (
          <Text style={styles.emptyRows}>There are no todos at the moment</Text>
        ) : null}
        <ListView
          dataSource={this.state.dataSource}
          renderRow={(rowData) => (
            <View style={styles.todo}>
              <Text style={styles.todoText}>{rowData.todo}</Text>
              <TouchableHighlight
                style={styles.button}
                onPress={() => this.handleDeletePress(rowData.todo)}
              >
                <Text style={styles.buttonText}>
                  Done
                </Text>
              </TouchableHighlight>
            </View>
          )}
          enableEmptySections={true}
        />
      </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#FFFAF6',
    marginTop: (Platform.OS === 'ios') ? 20 : 0,
    marginBottom: 50,
  },
  title: {
    backgroundColor: '#33ccff',
    padding: 15,
    color: 'white',
    fontSize: 24
  },
  emptyRows: {
    color: '#ccc',
    fontSize: 20,
    padding: 20
  },
  todo: {
    flex: 1,
    flexDirection: 'row'
  },
  todoText: {
    color: '#333',
    fontSize: 20,
    padding: 20,
    flex: 7
  },
  button: {
    height: 35,
    backgroundColor: '#4FB0A0',
    margin: 15,
    justifyContent: 'center',
    borderRadius: 5,
    flex: 3
  },
  buttonText: {
    fontSize: 18,
    fontWeight: 'bold',
    color: 'white',
    textAlign: 'center',
  }
});
