import React, { useState } from 'react';

import { makeStyles, fade } from '@material-ui/core/styles';

import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Drawer from '@material-ui/core/Drawer';

import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import IconButton from '@material-ui/core/IconButton';
import MenuIcon from '@material-ui/icons/Menu';
import Container from '@material-ui/core/Container';
import Paper from '@material-ui/core/Paper';
import SearchIcon from '@material-ui/icons/Search';
import InputBase from '@material-ui/core/InputBase';

import CharactersPage from './pages/CharactersPage';
import ComicsPage from './pages/ComicsPage';

const useStyles = makeStyles(theme => ({
  list: {
    width: 250,
  },
  root: {
    flexGrow: 1,
  },
  menuButton: {
    marginRight: theme.spacing(2),
  },
  title: {
    flexGrow: 1,
    display: 'none',
    [theme.breakpoints.up('sm')]: {
      display: 'block',
    },
  },
  paper: {
    padding: theme.spacing(1, 2),
  },
  search: {
    position: 'relative',
    borderRadius: theme.shape.borderRadius,
    backgroundColor: fade(theme.palette.common.white, 0.15),
    '&:hover': {
      backgroundColor: fade(theme.palette.common.white, 0.25),
    },
    marginLeft: 0,
    width: '100%',
    [theme.breakpoints.up('sm')]: {
      marginLeft: theme.spacing(1),
      width: 'auto',
    },
  },
  searchIcon: {
    width: theme.spacing(7),
    height: '100%',
    position: 'absolute',
    pointerEvents: 'none',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
  },
  inputRoot: {
    color: 'inherit',
  },
  inputInput: {
    padding: theme.spacing(1, 1, 1, 7),
    transition: theme.transitions.create('width'),
    width: '100%',
    [theme.breakpoints.up('sm')]: {
      width: 120,
      '&:focus': {
        width: 200,
      },
    },
  }
}));

function SideMenu({ classes, onSelected }) {
  return (
    <div
      className={classes.list}
      role="presentation"
    >
      <List>
        <ListItem button onClick={() => onSelected('characters')}>
          <ListItemText primary='Personagens' />
        </ListItem>
        <ListItem button onClick={() => onSelected('comics')}>
          <ListItemText primary='Revistas' />
        </ListItem>
      </List>
    </div>
  );
}

function App() {
  const classes = useStyles();
  const [searchString, setSearchString] = useState('');
  const [currentPage, setCurrentPage] = useState('characters');
  const [openDrawer, setOpenDrawer] = useState('');

  function handleSearch(event) {
    setSearchString(event.target.value);
  }

  function handleMenuClick(page) {
    setCurrentPage(page)
    setOpenDrawer(false)
  }

  function toggleDrawer(open) {
    setOpenDrawer(open)
  }

  return (
    <div className={classes.root}>
      <AppBar position="static">
        <Toolbar variant="dense">
          <IconButton edge="start" 
              className={classes.menuButton} 
              color="inherit" 
              aria-label="menu"
              onClick={() => toggleDrawer(true)}>
            <MenuIcon />
          </IconButton>
          <Typography className={classes.title} variant="h6" noWrap>
            MARVEL
          </Typography>

          <div className={classes.search}>
            <div className={classes.searchIcon}>
              <SearchIcon />
            </div>
            <InputBase
              placeholder="Search…"
              classes={{
                root: classes.inputRoot,
                input: classes.inputInput,
              }}
              inputProps={{ 'aria-label': 'search' }}
              onChange={(evt) => handleSearch(evt)}
            />
          </div>
        </Toolbar>
      </AppBar>
      <Paper elevation={0} className={classes.paper}>
        <Container maxWidth="lg">
          {currentPage == 'characters' && <CharactersPage searchString={searchString} />}
          {currentPage == 'comics' && <ComicsPage searchString={searchString} />}
        </Container>
      </Paper>

      <Drawer open={openDrawer} onClose={() => toggleDrawer(false)}>
        <SideMenu onSelected={handleMenuClick} classes={classes} />
      </Drawer>
    </div>
  );
}

export default App;
