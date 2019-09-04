import React, { useEffect, useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import GridList from '@material-ui/core/GridList';
import GridListTile from '@material-ui/core/GridListTile';
import ListSubheader from '@material-ui/core/ListSubheader';
import Grid from '@material-ui/core/Grid';
import { CharacterFactory } from '../factories/characterFactory';
import CharacterCard from '../components/CharacterCard';

const useStyles = makeStyles(theme => ({
  root: {
    flexGrow: 1,
  },
  icon: {
    color: 'rgba(255, 255, 255, 0.54)',
  },
}));

export default function HomePage() {
  const [characters, setcharacters] = useState([]);

  useEffect(() => {
    async function loadCharacters() {
      const data = [
        {
          "id":1010727,
          "name":"Spider-dok",
          "description":"",
          "resourceURI":"http://gateway.marvel.com/v1/public/characters/1010727",
          "thumbnail":
          {
            "path":"http://i.annihil.us/u/prod/marvel/i/mg/b/40/image_not_available",
            "extension":"jpg"
          }
        },
        {
          "id":1009610,
          "name":"Spider-Man",
          "description":"Bitten by a radioactive spider, high school student Peter Parker gained the speed, strength and powers of a spider. Adopting the name Spider-Man, Peter hoped to start a career using his new abilities. Taught that with great power comes great responsibility, Spidey has vowed to use his powers to help people.",
          "resourceURI":"http://gateway.marvel.com/v1/public/characters/1009610",
          "thumbnail":
          {
            "path":"http://i.annihil.us/u/prod/marvel/i/mg/3/50/526548a343e4b",
            "extension":"jpg"
          }
        }
      ];
      setcharacters(data.map(item => CharacterFactory(item)))
    }
    loadCharacters();
  })

  const classes = useStyles();
  return (
    <div className={classes.root}>
      <Grid container spacing={3}>
        {characters.length > 0 && characters.map((tile, index) => (
          <Grid item xs={2}>
            <CharacterCard character={tile} />
          </Grid>
        ))}
      </Grid>
    </div>
  );
}